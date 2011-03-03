/* Copyright 2009 HPDI, LLC
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using Hpdi.VssLogicalLib;
using System.Collections.Generic;
using System.IO;

namespace Hpdi.Vss2Git
{
    enum RecursionStatus
    {
        Continue, Skip, Abort
    }

    delegate RecursionStatus VssProjectCallback(VssProject project);

    delegate RecursionStatus VssFileCallback(VssProject project, VssFile file);

    /// <summary>
    /// Helper methods for working with VSS objects.
    /// </summary>
    /// <author>Trevor Robinson</author>
    static class VssUtil
    {
        public static RecursionStatus RecurseItems(
            VssProject project, VssProjectCallback projectCallback, VssFileCallback fileCallback)
        {
            if (projectCallback != null)
            {
                RecursionStatus status = projectCallback(project);
                if (status != RecursionStatus.Continue)
                {
                    return status;
                }
            }
            foreach (VssProject subproject in project.Projects)
            {
                RecursionStatus status = RecurseItems(
                    subproject, projectCallback, fileCallback);
                if (status == RecursionStatus.Abort)
                {
                    return status;
                }
            }

            HashSet<string> locallyProcessedFiles = new HashSet<string>();
            foreach (VssFile file in project.Files)
            {
                RecursionStatus status = fileCallback(project, file);
                locallyProcessedFiles.Add(file.PhysicalName);
                if (status == RecursionStatus.Abort)
                {
                    return status;
                }
            }
            foreach (VssRevision revision in project.Revisions)
            {
                var namedAction = revision.Action as VssNamedAction;
                if (namedAction != null && !namedAction.Name.IsProject)
                {
                    string physicalName = namedAction.Name.PhysicalName;
                    if (!locallyProcessedFiles.Contains(physicalName))
                    {
                        VssFile file = project.GetHistoricalFile(physicalName, namedAction.Name.LogicalName);
                        locallyProcessedFiles.Add(file.PhysicalName);
                        if (File.Exists(file.PhysicalPath) && File.Exists(file.DataPath))
                        {
                            RecursionStatus status = fileCallback(project, file);
                            if (status == RecursionStatus.Abort)
                            {
                                return status;
                            }
                        }
                    }
                }
            }
            return RecursionStatus.Continue;
        }
    }
}
