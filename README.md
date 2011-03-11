<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.01//EN" "http://www.w3.org/TR/html4/strict.dtd">
<html>
<head>

  <meta content="text/html; charset=ISO-8859-1" http-equiv="content-type">
  <title>Vss2Git</title>


</head>


<body>

<h1>Vss2Git</h1>

<p>Written by Trevor Robinson (<a href="mailto:trevor@scur%7CREMOVETHIS%7Crilous.com">trevor@scur<span style="display: none;">|REMOVETHIS|</span>rilous.com</a>).<br>

Copyright &copy; 2009 <a href="http://www.hpdi.com/">HPDI,
LLC</a>.<br>

Product and company names mentioned herein may be trademarks of their
respective owners.</p>

<h2>What is it?</h2>

<p>The Vss2Git project contains several components:</p>

<ul>

  <li><span style="font-weight: bold;">Vss2Git</span>
is a Windows GUI application that exports all or
parts of an existing <a href="http://www.microsoft.com/">Microsoft</a>
    <a href="http://msdn.microsoft.com/en-us/library/ms950420.aspx">Visual
SourceSafe 6.0</a> (VSS) [<a href="http://en.wikipedia.org/wiki/Visual_SourceSafe">Wikipedia</a>]
repository to a new <a href="http://git-scm.com/">Git</a>
repository. It attempts to construct
meaningful changesets (i.e. <a href="http://www.kernel.org/pub/software/scm/git/docs/user-manual.html#commit-object">Git
commits</a>) based on chronologically
grouping individual project/file revisions.</li>

  <li><span style="font-weight: bold;">VssDump</span>
is a console-based diagnostic tool that prints a
plain-text dump of the contents of a&nbsp;VSS repository.</li>

  <li><span style="font-weight: bold;">VssLogicalLib</span>
provides a .NET API for reading the contents
and history of a&nbsp;VSS repository.</li>

  <li><span style="font-weight: bold;">VssPhysicalLib</span>
is a set of low-level classes for reading
the various data files that make up a&nbsp;VSS database.</li>

  <li><span style="font-weight: bold;">HashLib</span>
is a generic stateless hashing API that currently
provides 16- and 32-bit <a href="http://en.wikipedia.org/wiki/Cyclic_redundancy_check">CRC</a>
generation.</li>

</ul>

<p>All components are written in C# using the Microsoft <a href="http://msdn.microsoft.com/en-us/netframework/default.aspx">.NET
Framework 3.5</a>.</p>

<h2>How is it licensed?</h2>

<p>Vss2Git is open-source software, licensed under the <a href="LICENSE.html">Apache License, Version 2.0</a> (<a href="LICENSE.txt">plain text</a>).
Accordingly,&nbsp;<span style="font-weight: bold;">any
use of the
software is <span style="text-decoration: underline;">at
your own risk</span></span>. Always back up your VSS
database
regularly, and especially before attempting to use this software with
it.</p>

<h2>What are its goals?</h2>

<p>Several key features not found in other VSS migration tools
inspired this project:</p>

<ul>

  <li><span style="font-weight: bold;">Preserving as
much history as possible from the VSS database</span>, including
deleted and renamed files. Vss2Git replays the history of the VSS
database from the very beginning, so it is possible to reconstruct any
prior version of the tree. Only explicitly destroyed or externally
archived (but not restored) history should be lost. Ideally, a migrated
VSS database should never need to be consulted again.</li>

  <li><span style="font-weight: bold;">Making
historical changes easily comprehensible</span>. Migration tools
that simply do a one-pass traversal of the files currently in the
repository, replaying all the revisions of each file as it is
encountered, generate version history that is difficult to correlate
among multiple files. Vss2Git scans the entire repository for
revisions, sorts them chronologically, and groups them into
conceptually changesets, which are then committed in chronological
order. The resulting repository should appear as if it
were&nbsp;maintained in Git right from the beginning.</li>

  <li><span style="font-weight: bold;">Robustness,
recoverability, and minimal user intervention</span>.
Vss2Git aims to be robust against common VSS database inconsistencies,
such as missing data files, so that migration can proceed unattended.
However, serious errors, such as Git reporting an error during commit,
necessarily suspend migration. In such cases, the user is presented
with an
Abort/Retry/Ignore dialog, so that manual intervention is an option.</li>

  <li><span style="font-weight: bold;">Speed</span>.
Vss2Git takes negligible CPU time. It can scan and build changesets for
a 300MB+ VSS database with 6000+ files and 13000+ revisions in about 30
seconds (or under 2 seconds if the files are cached) on a modern
desktop machine. Total migration
time is about an hour, with 98% of the time spent in Git.<span style="font-weight: bold;"></span></li>

</ul>

<p>Admittedly, some potentially interesting features are
currently outside the scope of the project, such as:</p>

<ul>

  <li><span style="font-weight: bold;">Incremental
migration</span>. Vss2Git currently always exports the entire
history of the selected files, and it does not attempt to handle
conflicts with files already in the Git repository prior to migration.</li>

  <li><span style="font-weight: bold;">Handling of
corrupt databases</span>. Vss2Git will fail to process VSS data
files with CRC errors. If you encounter such errors, run the VSS
Analyze.exe tool with the "-f" option. Make sure to back up your
database first. </li>

</ul>

<h2>How well tested is it?</h2>

<p><span style="font-weight: bold; text-decoration: underline;">This
code has not been extensively tested.</span>&nbsp;Vss2Git
was developed in about 2 weeks with the primary
purpose of migrating HPDI's VSS database to Git. With more than 300MB
of data and 13000 revisions committed over 7 years, that should be
reasonably representative of a large repository, but it is only one
dataset. If you decide to use Vss2Git, please let me know how it works
for you, and if you'd like me to add stats for your database here.</p>

<h2>Usage tips</h2>

<ul>

  <li>Run Vss2Git on a local <span style="font-weight: bold; text-decoration: underline;">backup
copy</span> of your repository. Not only will this avoid
jeopardizing your production repository, the migration will run much
faster accessing a local copy than one on a network share.</li>

  <li>Real-time virus/malware scanners, including Windows
Defender, can
interfere with Git updating its index file, causing it to fail with
errors like "fatal: Unable to write new index file". You may need to
configure these tools to exclude scanning the output Git repository
path if possible, or temporarily disable them if not.</li>

  <li>Generally, the Git output directory should be empty or
non-existent. When re-running the migration, you should delete
everything in the directory,&nbsp;including the .git subdirectory.
(Vss2Git doesn't do this for you for two reasons: 1) to avoid
accidental data loss, and 2) to allow merging of repositories.)
Vss2Git currently uses "git add -A" when committing changes, so any
untracked files that happen to be present will be included in the first
commit.</li>

  <li>Migration can start at any&nbsp;project in the VSS
database and includes all subprojects. VSS paths start with "$" as the
root project, with subproject names separated by forward slashes (e.g.
"$/ProjectA/Subproject1").</li>

  <li>You can exclude files by name from the migration by listing
patterns in the dialog box. The patterns are separated by semicolons
and may include the following wildcards:</li>

  <ul>

    <li>"?" matches any single character except a slash or
backslash.</li>

    <li>"*" matches zero or more characters except&nbsp;slash
or backslash.</li>

    <li>"**" matches zero or more characters (including slash and
backslash).</li>

  </ul>

  <li>VSS has some features that have no&nbsp;analogous
feature in Git. For instance:</li>

  <ul>

    <li>Branched files are simply copied. While Git will avoid
storing a redundant copy of the file in its database (since the content
hash will be identical), Git does not track that the file was copied.
(However, "git log -C", and especially, "git log --find-copies-harder",
can be used to locate copies after the fact.)</li>

    <li>Similarly, shared files are not directly supported.
Vss2Git will write each revision of a shared file to each directory it
is shared in, but once migration is complete, future changes must be
kept in sync by the user. Git technically supports using symlinks to
achieve a similar effect, but by default on Windows, they are checked
out as plain files containing a text link.</li>

    <li>Directories are not first-class objects in Git, as they
are in VSS. They are simply tracked as part of the path of the files
they contain. Consequently, actions on empty directories are not
tracked.</li>

    <li>VSS labels are applied to specific projects. Vss2Git
translates these as Git tags, which are global to the repository.</li>

  </ul>

</ul>

<h2>Known issues</h2>

<ul>

  <li>The Git executable needs to be on the Windows search path
(i.e. the PATH environment variable).</li>

  <li>Currently, only one VSS project path is supported. To
include disjoint
subtrees of your database, you'll need to run Vss2Git multiple times.
Unfortunately, this means that the commits won't be in chronological
order overall, and that commits containing files from separately
migrated projects will never be combined.</li>

  <li>Vss2Git includes a simplistic algorithm for generating
author
email addresses from VSS user names: it converts the name to lower
case, replaces spaces with periods, and appends the domain name
specified in the dialog box. For example, "John Doe" becomes
"john.doe@localhost". This is adequate for many cases, but obviously
not all. For now, you may wish to hack GitExporter.GetEmail().</li>

  <li>Git has difficulty dealing with changing the case of a
filename
on a case-insensitive file system (e.g. Windows). Vss2Git does contain
a workaround for this, which involves executing "git mv" twice, once to
rename to a temporary name, and then to rename to the final name. This
worked for me with msysgit 1.6.2, but there's no guarantee it will work
in all cases or with all versions.</li>

</ul>

<h2>Screenshot</h2>

<img src="Vss2Git.png" style="width: 600px; height: 393px;" alt="Screenshot">
<h2>Resources</h2>

<p>The following links may be useful to anyone migrating from VSS
and/or to Git. If Vss2Git does not meet your needs, perhaps one of the
other migration tools listed will.</p>

<ul>

  <li>Primary Git sites:&nbsp;<a href="http://git.or.cz/index.html">git.or.cz</a>
or&nbsp;<a href="http://git-scm.com/">git-scm.com</a></li>

  <li><a href="http://www.kernel.org/pub/software/scm/git/docs/user-manual.html">Git
User's Manual</a></li>

  <li><a href="http://code.google.com/p/msysgit/">msysgit:
Git on Windows</a></li>

  <li>Brett Wooldridge's <a href="http://www.riseup.com/%7Ebrettw/dev/VSS2Subversion.html">VSS-to-Subversion
Conversion</a> page</li>

  <li>Power Admin's <a href="http://www.poweradmin.com/sourcecode/vssmigrate.aspx">Visual
Source Safe (VSS) to Subversion (SVN) Migration</a> page</li>

  <li><a href="http://www.pumacode.org/projects/vss2svn/wiki">Vss2Svn</a>
project at <a href="http://www.pumacode.org/">PumaCode.org</a></li>

  <li>Alexander Gavrilov's <a href="http://github.com/angavrilov/git-vss/">git-vss</a>
("incremental exchange of simple changes between Git and VSS")</li>

</ul>

<h2>Release Notes</h2>

<p>1.0.10 &ndash; 6 Sep 2010 (Bug fixes based on patches from Matthias Luescher)</p>

<ul>

  <li>Format commit date/time according to ISO 8601 to avoid locale issues</li>

  <li>Set committer environment variables on commit, in addition to author</li>

  <li>Add option to force usage of annotated tags</li>

  <li>Naming and initial version fixes for branched files</li>

  <li>(Re)write target file if a branching action is applied to a project</li>

  <li>Do not delete files that have already been replaced by a new file with the same logical name</li>

  <li>Do not try to rename files that have already been deleted</li>

  <li>Support .Net 4 by disambiguating reference to VssPhysicalLib.Action</li>

</ul>

<p>1.0.9 &ndash; 18 Aug 2009</p>

<ul>

  <li>Suppress all actions against destroyed items (e.g. fixes "bad source" error from "git mv")</li>

  <li>Remove (empty) directory when a project is moved to a (subsequently) destroyed project</li>

  <li>Quote shell operators (&amp; | &lt; &gt; ^ %) when running git.cmd via cmd.exe</li>

  <li>Use a temporary file for comments containing newlines</li>

  <li>Skip "git add" and "git commit" for destroyed files</li>

  <li>Made "transcode comments" setting persistent</li>

</ul>

<p>1.0.8 &ndash; 14 Aug 2009</p>

<ul>

  <li>Fixed handling of projects restored from an archive</li>

  <li>Fixed handling of labels that differ only in case</li>

  <li>Fixed handling of label comments (implemented incorrectly in 1.0.7)</li>

  <li>Fixed FormatException in reporting unexpected record signature</li>

  <li>Improved reporting of errors during revision analysis</li>

  <li>Added RecordException base class to VssPhysicalLib</li>

  <li>Added RecordTruncatedException, which wraps EndOfBufferException while reading records</li>

  <li>Added commit date/time and user for tags</li>

  <li>Added VSS2Git version in log output</li>

</ul>

<p>1.0.7 &ndash; 22 Jul 2009</p>

<ul>

  <li>Fixed reading comments for labels</li>

  <li>Ignore empty labels</li>

  <li>Added support for labels and filenames that start with dashes</li>

  <li>Create all subdirectories for a project when it becomes rooted</li>

  <li>Explicitly add files to Git as they are created, to make them visible to subsequent directory operations</li>

</ul>

<p>1.0.6 &ndash; 22 Jul 2009</p>

<ul>

  <li>Quote temporary file path on Git command line if it includes spaces</li>

  <li>Support case-only renames for empty and parent projects</li>

</ul>

<p>1.0.5 &ndash; 17 Jun 2009</p>

<ul>

  <li>Ensure tag names are globally unique by appending a number</li>

</ul>

<p>1.0.4 &ndash; 16 Jun 2009</p>

<ul>

  <li>Configurable VSS database text encoding</li>

  <li>Optionally transcode Git comments to UTF-8</li>

  <li>Automatically configure Git repository for non-UTF-8
encoding</li>

  <li>Added output encoding support to VssDump</li>

  <li>Improved changeset building to include file/project
creation comments</li>

</ul>

<p>1.0.3 &ndash; 14 Jun 2009</p>

<ul>

  <li>Ignore file edits to unrooted projects (fixes "Value cannot
be null. Parameter name: path1" error)</li>

  <li>Ignore tags before initial commit (fixes "Failed to resolve
'HEAD' as a valid ref" error)</li>

  <li>Write VSS label names to log if they differ from the tag
name</li>

</ul>

<p>1.0.2 &ndash; 5 Jun 2009</p>

<ul>

  <li>Log full exception dumps</li>

  <li>Log root project and exclusion list</li>

  <li>Log changeset ID when dumping each changeset</li>

  <li>Save settings before migration</li>

</ul>

<p>1.0.1 &ndash; 4 Jun 2009</p>

<ul>

  <li>Search PATH variable for git.exe or git.cmd</li>

  <li>Strip illegal characters from tag names</li>

  <li>Improved error reporting</li>

</ul>

<p>1.0 &ndash; 22 Apr 2009</p>

<ul>

  <li>Initial release</li>

</ul>

</body>
</html>
