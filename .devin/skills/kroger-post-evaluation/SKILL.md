---
name: kroger-post-evaluation
description: Comprehensively evaluate a provided post and determine whether its relevant in assessing the health of Kroger.
argument-hint: "<PostURI>"
---

# Evaluation Definitions
Refer to `wiki/definitions.md` and familiarize yourself with the following definitions:
- Kroger
- Post Visibility
- Post Source
- Post Sentiment
- Post Interestingness
- Web Extraction Guidelines

# Workflow
Given a `<PostURI>`, produce one result line for the post plus zero or more result lines for notable comments. Each line carries a PV score (1-10), a PS inference, a sentiment score (1-10), and an PI score (1-10).

Steps:
1. Collect the post, its media, its metrics, and its comments (see [Data Acquisition]).
2. Score the post: visibility, source, sentiment, interestingness.
3. Score the comments and fold their consensus into the post sentiment.
5. Emit output in the exact [Output Format], and nothing else.

# Data Acquisition
1. Use the Playwright MCP tools to open the post and read it. Many sites block plain fetching, so do not rely on simple HTTP fetches as the first choice.
2. View every image or screenshot with the read tool. A title alone is not enough.
3. Collect the post title, body, author, timestamp, and all engagement metrics, including crossposts or reposts.
4. Collect comments. Prefer sorting by top and expand nested replies for the highest-scoring threads.
5. For batch runs where several agents share one browser, a session-authenticated JSON endpoint may be used instead, with cookies taken from the Playwright browser context and kept in a temporary file. Never print, log, or commit cookie values.

# Output Format
Emit only the result lines. Write no headings, prose, justifications, or code fences around them.

## Post line
```text
[URI], [Post Title], [Post Creation Time], [Post Analysis Time], [Post Interestingness], [Initial Post Visibility], [Projected Post Visibility], [Post Source], [Source Confidence], [Post Sentiment]
```

Fields:
- `[URI]` - The URI of the post
- `[Post Title]` — pithy, informative summary of at most 20 words. Use the original title only if it already describes the content. No square brackets inside.
- `[Post Creation Time]` - The time the post was created in ISO-8061 format.
- `[Post Analysis Time]` - The current time in ISO-8061 format.
- `[Post Interestingness]` - PI score (1-10)
- `[Initial Post Visibility]` - PV score (1-10)
- `[Projected Post Visibility]` - PV score (1-10)
- `[Post Source]` - The source of the post
- `[Source Confidence]` - A percentage (0-100) indicating the confidence in the source inference. Do not include the % symbol.
- `[Post Sentiment]` - A score from 1-10 indicating the sentiment of the post

Example:

```text
x.com/u/status/1, Kroger plans to close Ohio stores, 16:30:55Z, 19:48:19Z, 8, 2, 3, Manager, 80, 2
```

## Comment line
Print comment lines directly below their post line, one per line, ordered by interest, at most 8 per post.

```text
[URI], [Comment Title], [Comment Creation Time], [Comment Analysis Time], [Comment Interestingness], [Comment Visibility], [Comment Source], [Comment Confidence], [Comment Sentiment]
```

Fields:
- `<CommentURI>` — a permalink that leads directly to the comment.
- `[Comment Description]` — a pithy summary, at most 20 words, faithful to what the comment says.
- Remaining fields follow the post-line rules.

Comment URI patterns (use IDs taken from the retrieved data):
- Reddit:
  `https://www.reddit.com/r/<sub>/comments/<post_id>/comment/<comment_id>/`
- X:
  `https://x.com/<user>/status/<reply_id>`

Never construct or recall a comment identifier from memory. If a `<CommentURI>` does not resolve to a comment whose text matches its description, the output will be rejected — verify each one.

## Error line
Use it when the post cannot be evaluated (deleted, private, blocked, or unsupported). Do not fabricate metrics.

```text
ERROR <PostURI>: [Reason]
```

# Validation Checklist
Before responding, confirm every item:
- [ ] Each output line matches the format exactly, with square brackets, parentheses, and integers as specified.
- [ ] PV, sentiment, and PI are given as integers between 1 and 10.
- [ ] PS matches the valid sources specified in `definitions.md`.
- [ ] Every image was viewed, and the description reflects its content.
- [ ] Every `<CommentURI>` was taken from retrieved data and resolves to a comment whose text matches its description.
- [ ] Sentiment reflects implications for Kroger, includes the comment consensus.
- [ ] PI is reflective of how useful the post is for our informational edge.
- [ ] The output contains no extra prose.