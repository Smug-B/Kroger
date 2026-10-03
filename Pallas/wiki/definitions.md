# Kroger
The Kroger Company (sometimes referred to as just The Kroger Co. or just Kroger) is a major American supermarket chain with numerous subsidiaries and affiliates. Kroger trades under the ticker $KR, and thus is also commonly abbreviated as "KR". Because the Kroger Company shares the same name as its key subsidiary, it should be assumed that "Kroger" or "KR" refer to the entire company operations unless otherwise specified.

## Subsidiaries and Affiliates
- Kroger
- Dillons
- Baker's
- Gerbes
- Food 4 Less
- Foods Co.
- Ralphs
- Fry's Food & Drug Stores
- Fry's Marketplace
- Fred Meyer
- Fred Meyer Jewelers
- Barclay's Jewelers
- Littman Jewelers
- Home Chef
- King Soopers
- City Market
- Pay Less
- Smith's Food and Drug
- Harris Teeter
- QFC (Quality Food Centers)
- Roundy's Supermarkets
- Metro Market
- Pick 'n Save
- Mariano's
- The Little Clinic
- JayC Food Stores
- Ruler Foods
- Vitacost
- Kroger Health
- Kroger Personal Finance
- Kroger Technology
- Kroger Manufacturing
- Kroger Brands
- Kroger Logistics

## Competitors
Analyzing Kroger's competitors is crucial to understanding the company's position in the market and its potential for future growth or decline. 

Competitors include, but are not limited to:
- ALDI
- LIDL
- Walmart
- Sam's Club
- BJ's Wholesale
- Costco
- Target
- Whole Foods Market
- Sprouts
- Trader Joe's
- Safeway
- Publix
- Wegmans

# Web Extraction Guidelines
1. **Never Simply Fetch:** Many websites do not allow for programmatic access to their content. Instead, you should use your Playwright tool to navigate to the post and extract the information you need.
2. **Never Guess Metrics:** Requested metrics must be extracted via precise CSS selectors. If the selector fails, report the failure instead of estimating.
3. **Prefer DOM Evaluation:** Use JavaScript execution (`page.evaluate`) to pull attributes (`href`, `data-score`) rather than parsing unstructured markdown text dumps.
4. **Log-In Requests:** Certain sites restrict access to their content behind a login. If you encounter such a site, you should pause operations and request the user to provide login credentials. After the user has provided credentials, or logged-in on your behalf, you can continue work.
5. **Handle Anti-Bot Stubs:** If a page returns an empty shell or a Cloudflare challenge, stop immediately and report that the page content could not be rendered, rather than hallucinating what the post *might* have said based on the URL.

# Scripts
Within `./Kroger/Pallas/scripts` there are several scripts that you may be requested to use. 

A brief definition of each script is provided below:

## log_encounter.py
`log_encounter.py` takes the following arguments:
- `uri`: URI leading to the post (REQUIRED)
- `parent_uri`: If the post is a comment, uri leading to the parent post. If the post is a top-level post, uri leading to itself (REQUIRED)
- `create_time`: Post's creation time in ISO-8601 format i.e. `YYYY-MM-DD HH:MM:SS` (REQUIRED)
- `visibility`: Calculated post visibility (REQUIRED)
- `source`: Inferred post source (REQUIRED)
- `source_confidence`: Confidence of inferred post source (REQUIRED)
- `content`: Pithy summary of post's content (REQUIRED)
- `run_name`: Name of folder where the encounters database will be stored (optional)

It should be noted that `run_name` is optional, but if not provided, the script will default to using the current date and time as the run name.
This has the possibility of contaminating the encounters database with encounters from different runs, which is not ideal.
Thus, you should always supply `run_name` as the date of your instantiation.

This script logs "encounters" in a relational database powered by SQLite. An encounter is a post that has been discovered and briefly analyzed, providing the visibility metric as well as a source inference and a content summary.

The database is stored under `./Kroger/Pallas/runs/[run_name]/encounters.db`, where [run_name] refers to the parameter passed to the script.

`log_encounter.py` currently creates two tables in the database: `encounters_map` and `comments_map`.
If `log_encounter.py` is called on a comment with a `parent_uri` that does not exist in the `encounters_map` table, it will create an entry for the parent post in the `encounters_map` table. However, this entry will only populate the `uri` and `id` columns. 

### encounters_map
Contains encounter data only for top level posts. `id` is the primary key, but `uri` is also a unique identifier for the post.

Columns:
- `id`: Auto-incrementing primary key
- `uri`: Post uri and unique identifier
- `create_time`: Post's creation time
- `encounter_time`: Time the post was logged by calling `log_encounter.py`
- `visibility`: Calculated post visibility
- `source`: Inferred post source
- `source_confidence`: Confidence of inferred post source
- `content`: Pithy summary of post's content

### comments_map
Contains encounter data only for comments. Comments are linked to their parent post via the `parent_id` column, which references the `id` column in `encounters_map`.

Columns:
- `comment_id`: Auto-incrementing primary key
- `parent_id`: ID of the parent post. References `id` in `encounters_map`
- `uri`: Comment uri and unique identifier
- `create_time`: Comment's creation time
- `encounter_time`: Time the comment was logged by calling `log_encounter.py`
- `visibility`: Calculated comment visibility
- `source`: Inferred comment source
- `source_confidence`: Confidence of inferred comment source
- `content`: Pithy summary of comment's content

# Metrics
We define metrics to be used across post analysis.

## Evidence Quality
Whenever you are asked to provide a metric, you may be asked to justify your response with evidence. The quality of this evidence -- evidence quality, abbreviated as EQ -- is defined as follows:

| EQ Grade | Description |
| --- | --- |
| A | Primary or direct evidence |
| B | Credible first-hand source |
| C | Corroborated secondary source |
| D | Uncorroborated secondary source |
| E | Speculation or opinion |

## Post Visibility
Post visibility -- sometimes shortened to "visibility," or abbreviated as PV -- is a metric determining how many people have seen a post and interacted with it, such that the post has left an impression on them.

Determining PV varies based on the underlying social media platform as different metrics are exposed by different platforms. It's crucial to understand the platform's metrics when determining PV, as well as balancing the PV results from different platforms. For instance, a post with a high number of views on X (formerly Twitter) may not be as visible as a post with a high number of views on YouTube.

PV, when requested as a numerical value, should be given as a value ranging from 1 (least visible) to 10 (most visible). This value should be a reflection of the post's current visibility, not its potential for growth.

However, potential growth is also a useful consideration. We define "potential post visibility" (abbreviated as PPV) as PV, adjusting for possible visibility growth based on the post's current state as well as the time-elapsed since the post's creation. For instance, a post with PV determined to be 3 may have significant potential to grow in visibility if it was created just 10 minutes ago, but would have little potential if it was created over 24 hours ago.

PV (or PPV) may sometimes be requested in the form of a string. In these cases, the string outputted should be reflective of PV (or PPV) as follows:
| PV Label | PV Range |
| --- | --- |
| Low | Below 3 |
| Mid | 3 to 7 inclusive |
| High | Above 7 |

A shared documentation detailing how PV is calculated for each platform should be used maintained consistency within each platform, as well as across all platforms. Such a document is located under the `Social Media Platforms` header within this document. Changes to this section should be made as needed to ensure consistency, however each change suggestion should be reviewed by the user before being merged into the shared documentation.

Before performing any analysis or determination of PV (or PPV), you should always check if the shared documentation has been updated to reflect any changes in the calculation method. If it has, you should update your analysis or determination of PV (or PPV) accordingly.

## Post Source
Post source -- sometimes shortened to "source," or abbreviated as PS -- is an inference of who authored the post. Sources should fall into the following bands:

| Source | Description |
| --- | --- |
| Manager | Someone who is responsible for managing a store, as well as someone who may be from Kroger's corporate office. |
| Employee | Someone who works at a Kroger store, but is not a manager. |
| Customer | Someone who could shop at a Kroger store, and is not an employee or manager.|
| Media | Someone who is reporting on a Kroger store. |
| Other | Someone who does not fit into any of the above categories. |
| Unsure | Default option when you are unable to determine the source of the post. |

It is important that source inference is done so using evidence. 
You should grade your inferences through referring to evidence quality as defined in the `Evidence Quality` section of this document.
If multiple post sources are plausible, and evidence quality is similar (within one letter grade), you should explicitly state your uncertainty through the `Unsure` source option.

## Business Relevance
Business relevance -- sometimes shortened to "relevance," or abbreviated as BR -- is a numerical metric of how relevant the information contained within the post is to Kroger's business operations. 

| BR Score | Meaning |
| --- | --- |
| 5 | Highly relevant to Kroger's business operations |
| 4 | Potentially highly material |
| 3 | Potentially material |
| 2 | Plausible operational relevance |
| 1 | Weak or indirect relevance |
| 0 | No plausible mechanism |

It is important that business relevance is graded using evidence quality as defined in the `Evidence Quality` section of this document.

## Business Implications
Business implications -- sometimes shortened to "implications," or abbreviated as BI -- is a numerical metric quantifying how the information contained within the post may impact Kroger's business operations. 

| BI Score | Meaning |
| --- | --- |
| -2 | Highly negative impact on Kroger's business operations |
| -1 | Moderately negative impact on Kroger's business operations |
| 0 | Neutral impact on Kroger's business operations |
| 1 | Moderately positive impact on Kroger's business operations |
| 2 | Highly positive impact on Kroger's business operations |

## Post Tone
Post tone -- sometimes shortened to "tone" -- is a measure of how the post is presented to, or perceived by, the general public. Use the following bands:

| Tone Score | Meaning |
| --- | --- |
| -5 | Very negative |
| -3 to -4 | Negative |
| -1 to -2 | Slightly negative |
| 0 | Neutral |
| 1 - 2 | Slightly positive |
| 3 - 4 | Positive |
| 5 | Very positive |

## Information Value
Information value -- sometimes shortened to "information value," or abbreviated as IV -- is a numerical metric of how much potentially useful information the post adds to our understanding of Kroger's operations or competitive position, considering source quality, novelty, specificity, and potential business relevance. It determines whether the post is worth investigating further. Use the following bands:

| IV Score | Meaning |
| --- | --- |
| 7 - 10 | High possible information value. Definitely needs further investigation |
| 4 - 6 | Medium information value. Could investigate if nothing else is queued. |
| 1 - 3 | Low information value. Log and move on. |

## Social Media Platforms
### X
X (formerly Twitter) is a social media platform that allows users to post short messages, called "tweets," to a public feed. 

#### PV
x.com provides four possible metrics to derive visibility. 
Ranked in order of importance, they are as follows:
Reply
Repost
Likes
Views

Determining visibility from an aggregate of these four metrics can be a bit ambiguous.
However, a good heuristic to utilize is:
- 1 reply is worth 10 reposts
- 1 repost is worth 2 likes
- 1 like is worth 100 views

Here is an example of some x.com metrics that would correspond to a 10 in terms of
visibility: 100k likes. 12k reposts. 5k replies. 1.2mm views

Here is an example of some x.com metrics that would correspond to a 8 in terms of
visibility: 20k likes. 300 reposts. 100 replies. 800k views

Here is an example of some x.com metrics that would correspond to a 5 in terms of
visibility: 2k likes. 320 reposts. 50 replies. 200k views

Here is an example of some x.com metrics that would correspond to a 2 in terms of
visibility: 100 likes. 12 reposts. 3 replies. 3k views

### Reddit
Reddit is a social media platform that allows users to post content, called "posts," to a public feed. 

#### PV
reddit.com provides two core metrics to derive visibility: upvotes and comments.

A good heuristic to utilize for reddit is:
- 1 comment is worth 10 upvotes

Here is an example of some reddit.com metrics that would correspond to a 10 in terms of
visibility: 100k upvotes. 2k comments.

Here is an example of some x.com metrics that would correspond to a 8 in terms of
visibility: 5k upvotes. 800 comments.

Here is an example of some x.com metrics that would correspond to a 5 in terms of
visibility: 1k upvotes. 200 comments.

Here is an example of some x.com metrics that would correspond to a 2 in terms of
visibility: 20 upvotes. 2 comments.