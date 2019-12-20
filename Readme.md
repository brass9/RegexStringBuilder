#RegexStringBuilder

An implementation of Regex that works on StringBuilders, which are often [asked for on StackOverflow](https://stackoverflow.com/questions/3504850/regex-replacements-inside-a-stringbuilder/25029604#25029604) and other coding discussions.

It is worth being aware that Microsoft never implemented Regex this way for performance reasons that are not entirely clear to me. My best understanding of their assertions is that the Microsoft .Net implementation of StringBuilder "scans poorly," that is, a string is guaranteed to occupy a single contiguous block of memory, at least as far as the .Net CLR is aware, and, maybe Windows as well, I'm not clear on how deep that guarantee goes. However, StringBuilder is structured underneath to allow for quick changes, but, it might scan poorly. More on this below under Performance.

In short, though, the nice thing about this RegexStringBuilder is that it is drop-in compatible with the existing Regex implementation, so you can quickly performance test it and classic Regex, and proceed with what meets your performance needs.

#Origins - Mono

The Mono team until Apr 27, 2014 had need for a Regex implementation, and at the time Microsoft's implementation was closed source. A Novell/JVM implementation was adapted by the Mono team for C#, swapping Java's StringBuffer for C#'s StringBuilder. And so a strong Regex StringBuilder implementation was born.

I don't know enough about Mono history to know why it was deleted on Apr 27, 2014, but there seems to be a lot about removing anything related to the JVM at the time in the git history.

You can see the commit where this code was pulled from at commit 03aee949f13b7908f6fb797046c7b29f02196a17

Since the implementation was removed after that commit, this implementation is not tied to Mono's git history. This also makes grabbing this git repo much faster - Mono is LARGE.

The Regex in Mono was intended to implement System.Text.RegularExpressions.Regex, but now reusing it in plain .Net, that class name and namespace already exist, so we can't have them both sitting in the same namespace. So, any time "System" was used in the Mono namespaces, I've used "RegexStringBuilder.Mono" to avoid collision. For example, this Regex lives at RegexStringBuilder.Mono.Regex. However, to make it easier to find the main class, and avoid a situation where you can only use Regex or RegexSB conveniently in a given class file, RegexSB lives at the root (RegexStringBuilder.RegexSB) and simply inherits from the Mono class.


#Performance

Attempting to verify how frequent "might scan poorly" is, it's notable that StringBuilder is now open source, so you can read the source and see that if you allocate enough to it then write linearly, it seems like things remain contiguous in StringBuilder, and so, should scan quickly. Then again, the Microsoft implementation of Regex, which never uses StringBuilder underneath the hood and only strings, gets performance in part by leveraging CPU Vectors or chunks of characters at a time, matching them against masks, and perhaps that would be impossible with a StringBuilder.