# Kroger Monitoring Workflow - Iteration 1

## Overview
This workflow closely follows the example major/minor loop structure, with two modifications
justified below. It is executed via the shared Playwright MCP browser (authenticated to both
x.com and reddit.com).

## Search queries per minor loop

### Kroger terms
- `kroger` (catches "Kroger", "$KR" mentions in context)
- `kroger OR "king soopers" OR ralphs OR "fred meyer" OR "harris teeter" OR marianos OR fry's food OR qfc OR "pick n save"` (subsidiary expansion)

### Competitor terms
- `aldi OR lidl OR publix OR wegmans OR sprouts` (direct competitors)
- `walmart OR costco OR "sam's club" OR target` grocery context is implied by search relevance
Competitor searches are intentionally grouped into two broad OR-queries rather than one
query per competitor: individual competitor searches yield extremely high volume dominated
by unrelated content, while grouped queries surface grocery-sector comparison posts which
are the ones relevant to KR market share.

### Related terms
- `"grocery prices" OR "food prices" OR supermarket OR "grocery store"`

## Workflow outline

Major loop (repeats continuously):
1. Minor loop [x.com] [Kroger + subsidiaries]
   - https://x.com/search?q=<query>&f=live (Latest tab)
2. Minor loop [reddit] [Kroger + subsidiaries]
   - https://www.reddit.com/search/?q=<query>&sort=new
   - plus https://www.reddit.com/r/kroger/new/ (dedicated subreddit sweep)
3. Minor loop [x.com] [Competitors]
4. Minor loop [reddit] [Competitors]
5. Minor loop [x.com] [Related terms]
6. Minor loop [reddit] [Related terms]

Minor loop:
1. Navigate to the search URL, extract post URIs + visible engagement metrics via
   `browser_evaluate` DOM queries (per Web Extraction Guidelines: DOM evaluation preferred).
2. Filter out posts older than 7 days and posts already present in log.txt
   (with Revisit handling per the Delegation rules).
3. For new post URIs, delegate a `kroger-post-evaluation` subagent.
   Subagents share the single Playwright browser, so they run sequentially to avoid
   navigation conflicts. Each subagent receives a small batch (~5) of URIs and emits
   one post-analysis line per URI (plus comment lines) — this keeps delegation
   overhead proportional while preserving the per-post output contract.
4. Compile subagent output into log.txt rows.

## Differences from the example workflow
- Subagents run **sequentially** rather than in parallel because all subagents share one
  Playwright MCP browser instance; parallel navigation would corrupt each other's page state.
- Competitor queries are grouped (see above) to reduce noise.
- An explicit r/kroger subreddit sweep is added since it is the densest source of
  Kroger-specific posts (mostly employees/customers).
- Search result scraping happens in the manager (me) via DOM evaluation; subagents only
  receive concrete post URIs, keeping delegation payloads small and reliable.
