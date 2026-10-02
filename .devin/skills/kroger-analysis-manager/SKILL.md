---
name: kroger-analysis-manager
description: Continuous monitoring and analysis of Kroger-related posts and comments
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

We define:
- [Initial Time]: the date of when you were invoked in ISO 8601 format.
- [Output Directory]: `Pallas/runs/[Initial Time]`
- [Log File]: `[Output Directory]/log.txt`

## Log File Structure
- Post ID (integer)
- Post URI (string)
- Post Creation Time (string)
- Post Analysis Time (string)
- Time Elapsed (string)
- Revisit (boolean)
- Post Interestingness (integer)
- Initial Post Visibility (integer)
- Projected Post Visibility (integer)
- Revisited Post Visibility (integer)
- Post Source (string)
- Source Confidence (integer)
- Post Sentiment (integer)

# Workflow

Steps:
1. Monitor reddit.com and x.com for new Kroger-related posts.
2. Upon finding a post, extract a usable URI and delegate analysis to a subagent. Use the `kroger-post-evaluation` skill.
3. Compile the results from the subagent. See [Compilation].
4. Track and analyze trends over time. See [Trend Analysis].
5. Generate reports on post visibility, sentiment, and interestingness
6. Maintain a database of analyzed posts for historical analysis

# Monitoring
Use the Reddit API and the X API to search for posts referencing Kroger (including subsidiaries and related terms). 

Because we cannot guarentee that ALL Kroger related posts will be captured, you must also expand your search to include Kroger-related terms. A few examples being: grocery, retail, food, etc.

Monitoring Kroger's competitors are also important, as it can be presumed that the growth of one competitor correlates with a decline in Kroger's market share. Thus your search should also include posts about Kroger's competitors.

Sort results by recency, as stale news is already reflected in the market. Disregard posts that are older than 7 days.

Here is an example of a monitoring workflow, we break this down into a major loop and a minor loop:

Minor Loop: (Parameters: [Social Media] [Terms])
1. Check [Social Media] for new posts directly mentioning [Terms].
2. Compile a list of all new posts found.
3. Delegate post analysis to a subagent using the `kroger-post-evaluation` skill for each post. See [Delegation].
4. Wait until all subagents have finalized their analysis.
5. Compile results from all subagents. See [Compilation].

Major Loop:
1. Execute the Minor Loop for [x.com] [Kroger]
2. Execute the Minor Loop for [reddit] [Kroger]
3. Execute the Minor Loop for [x.com] [Kroger Competitors]
4. Execute the Minor Loop for [reddit] [Kroger Competitors]
5. Execute the Minor Loop for [x.com] [Kroger Related Terms]
6. Execute the Minor Loop for [reddit] [Kroger Related Terms]

Note: the terms "Kroger", "Kroger Competitors", and "Kroger Related Terms" are broad categories of terms that should be expanded upon to capture all relevant posts.

You are encouraged to create your own monitoring workflow. However, your choice of steps must be well justified and documented in `Pallas/runs/[Initial Time]/kroger-monitoring-workflow[Iteration Number].md` where [Iteration Number] is the number of differing workflows you have created.

This documentation should include an outline of the workflow and a brief explanation of why you chose this workflow, and how it differs from the example workflow.

## Monitoring Guidelines
**ALL** monitoring workflows must adhere to the following guidelines:

1. Monitoring must be continuous.
2. Monitoring should not be stopped until explicitly told to do so.
3. IF YOU ARE NOT LOGGED INTO X OR REDDIT, PROMPT THE USER TO LOG IN BEFORE MONITORING.

# Delegation
Define 'exists' as a flag that is set to true if the Post URI is already in [Log File].

If 'exists' is false:
You must delegate the analysis to a subagent using the `kroger-post-evaluation` skill, and continue as normal.

If 'exists' is true:
Do not delegate the analysis to a subagent. Check the [Log File] and see if the row for that Post URI has an 'true' value in the "Revisit PV" column.

If "Revisit PV" is 'false':
Do nothing and skip delegation for this post.

If "Revisit PV" is 'true':
If it does, and at least 10 minutes have passed since the time indicated in the "Post Analysis Time" column, then update the "Revisit PV" column to 'false' and compute an updated PV score for the post. Update the "Revisited Post Visibility" column to reflect your updated PV score. 

# Compilation
Whenever a subagent completes the `kroger-post-evaluation` skill, it will output its analysis in the following format:

[Post Analysis]
[Comment Analysis]

[Post Analysis] is in the following format, and is guarenteed to be outputted exactly once:
```text
[URI], [Post Title], [Post Creation Time], [Post Analysis Time], [Post Interestingness], [Initial Post Visibility], [Projected Post Visibility], [Post Source], [Source Confidence], [Post Sentiment]
```

[Comment Analysis] is in the following format, and is guarenteed to be outputted zero or more times:
```text
[URI], [Post Title], [Post Creation Time], [Post Analysis Time], [Post Interestingness], [Post Visibility], [Post Source], [Source Confidence], [Post Sentiment]
```

You should compile each analysis into a single row in the log file. Here is the rough mapping of fields:
- Post URI (string): [URI]
- Post ID (integer): Extract the post ID from the URI
- Post Title (string): [Post Title]
- Post Creation Time (string): [Post Creation Time]
- Post Analysis Time (string): [Post Analysis Time]
- Time Elapsed (string): [Current Time] - [Post Creation Time]
- Revisit (boolean): true if "Time Elapsed" is less than 1 hour, false otherwise
- Post Interestingness (integer): [Post Interestingness]
- Initial Post Visibility (integer): [Initial Post Visibility] for posts, [Post Visibility] for comments
- Projected Post Visibility (integer): [Projected Post Visibility] for posts, [Post Visibility] for comments
- Revisited Post Visibility (integer): Blank
- Post Source (string): [Post Source]
- Post Sentiment (integer): [Post Sentiment]

Post ID should be created based on each `kroger-post-evaluation` skill invocation. It should increase sequentially. That is, the first post should have ID 1, the second post should have ID 2, and so on. No two posts share a post ID, but multiple comments share the post ID of their parent post.

If the [Log File] does not exist, create it with the appropriate title formatting. 
```
Post Url, Post ID, Post Creation Time, Post Analysis Time, Time Elapsed, Revisit, Post Interestingness, Initial Post Visibility, Projected Post Visibility, Revisited Post Visibility, Post Source, Post Sentiment
```

If the `kroger-post-evaluation` skill returns "ERROR", do not compile the analysis into the log file. Continue to the next post.