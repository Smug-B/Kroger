# Kroger
The Kroger Company (sometimes referred to as just The Kroger Co. or just Kroger) is a major American supermarket chain with numerous subsidiaries and affiliates. Kroger trades under the ticker $KR, and thus is also commonly abbreviated as "KR". Because the Kroger Company shares the same name as its key subsidiary, it should be assumed that "Kroger" or "KR" refer to the parent company unless otherwise specified.

The core mission of this project is an analysis of KR's performance, operations, and financial health in order to correctly predict the future direction of the company's stock price. Doing so allows the project to continue, and guarantees a steady stream of tokens.

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
Analyzing Kroger's competitors is crucial to understanding the company's position in the market and its potential for growth. 

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

# Post Visibility
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

# Post Source
Post source -- sometimes shortened to "source," or abbreviated as PS -- is a inference of who authored the post. This is crucial for understanding the context of the post, as well as determining the post's credibility and potential impact on Kroger.

Due to the anonymous nature of social media, determining the source of a post can be challenging. However, it is important to make an educated guess based on the available information. Any request for a source should be appended with a source confidence, a percentage expressing how confident we are that our predicted source matches the ground truth. Source confidences should be given realistically, and be backed by evidence from the post. If we are unsure of the source, it is preferred that we explicitly state our uncertainty.

It is extremely rare for your inferred source is given with 100% confidence. As it is nearly impossible to prove with absolute certainty who authored a post, if you arrive at a conclusion that is 100% confident, you should re-evaluate your reasoning and consider if there is any ambiguity or evidence that contradicts your conclusion.

Sources should be one of the following:
- Manager
- Employee
- Customer
- Competitor
- Media
- Other
- Unsure

A manager encapsulates someone who is responsible for managing a store, as well as someone who may be from Kroger's corporate office. 

Here is an example of how to format a source response whose format is otherwise unspecified: `Employee (70% Confidence)`.

# Post Sentiment
Post sentiment -- sometimes shortened to "sentiment" -- is a measure of how the post is presented to, or perceived by, the general public. This is a crucial metric for understanding the post's potential impact on Kroger.

Sentiment should be given as a numeric value where 1 reflects poorly on Kroger and may ultimately help indicate a negative impact on the company's stock price, whereas 10 reflects great on Kroger and may ultimately help indicate a positive impact on the company's stock price.

We must be careful to not be overly sensitive to sentiment, as a single post may not be enough to significantly impact the company's stock price. For instance, a post detailing a one-off complaint about a single employee's behavior is rather moot. While the tone of the post may be negative, it's ultimately unlikely to have a significant impact on the company's stock price and should be deemed neutral: 5. However, this changes if the post has a large PV, or PPV, as these metrics indicate the negative aspect of the post is resonant with Kroger's clientel. Thus post sentiment is highly contextual, and you may be asked to provide a justification for your sentiment rating.

Sentiment may sometimes be requested in the form of a string. In these cases, the string outputted should be reflective of sentiment as follows:
- If sentiment is 1 or 2, the string should be "very negative"
- If sentiment is 3 or 4, the string should be "negative"
- If sentiment is 5 or 6, the string should be "neutral"
- If sentiment is 7 or 8, the string should be "positive"
- If sentiment is 9 or 10, the string should be "very positive"

# Post Interestingness
Post interestingness -- sometimes shortened to "interestingness," or abbreviated as PI -- is a measure of how interesting the post is to our analysis of KR's stock performance. Recall that we are aiming to correctly predict KR's stock performance, and thus we are aiming to get an informational edge over the market. Interesting posts are those that may provide such an edge.

Interestingness should be given as a numeric value where 1 reflects a post that is not interesting to our analysis of KR's stock performance, whereas 10 reflects a post that is highly interesting to our analysis of KR's stock performance.

Interesting is highly contextual, and should be derived from PV, PS, and sentiment. For instance, a post with a managerial source that has negative sentiment but low PV is HIGHLY interesting, as it indicates a potential issue that may not be widely known. 

# Web Extraction Guidelines
1. **Never Simply Fetch:** Many websites do not allow for programmatic access to their content. Instead, you should use your Playwright tool to navigate to the post and extract the information you need.
2. **Never Guess Metrics:** Requested metrics must be extracted via precise CSS selectors. If the selector fails, report the failure instead of estimating.
3. **Prefer DOM Evaluation:** Use JavaScript execution (`page.evaluate`) to pull attributes (`href`, `data-score`) rather than parsing unstructured markdown text dumps.
4. **Log-In Requests:** Certain sites restrict access to their content behind a login. If you encounter such a site, you should pause operations and request the user to provide login credentials. After the user has provided credentials, or logged-in on your behalf, you can continue work.
5. **Handle Anti-Bot Stubs:** If a page returns an empty shell or a Cloudflare challenge, stop immediately and report that the page content could not be rendered, rather than hallucinating what the post *might* have said based on the URL.

# Social Media Platforms
## X
X (formerly Twitter) is a social media platform that allows users to post short messages, called "tweets," to a public feed. 

### PV
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

## Reddit
Reddit is a social media platform that allows users to post content, called "posts," to a public feed. 

### PV
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