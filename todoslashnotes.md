============================================= 11/12/21 =========================================

TODO:

1. ~~Set up transcriptStore to store a list of transcript chunk GUIDS for the selected content and the current chunk index~~
2. ~~Components to represent Term, UserTerm, and AbstractTerm~~
3. ~~Components to represent a transcript chunk (how will term components be organized/ laid out?)~~
4. ~~Figure out a way to include whitespace and punctuation in how words are displayed w/o including either in the terms themselves~~
5. Figure out how the client will request & update UserTerm details

OTHER ISSUES:

1. Routing outside of TSX (redirecting to home page after successsful login etc)
2. Need loading components, esp. for content feed

================================================================================================

============================================= 11/13/21 =========================================

FROM YESTERDAY:

4. ~~Figure out a way to include whitespace and punctuation in how words are displayed w/o including either in the terms themselves~~
5. Figure out how the client will request & update UserTerm details

TODO:
1. ~~Create endpoint to get trailing characters for a term within a transcript chunk~~ 
    
    -Maybe add a 'TrailingCharacters' prop to AbstractTermDto?
2. ~~Make sure that Terms/UserTerms are stored as normalized strings and retrieved based on the normalized value of the request string~~

    ~~-NOTE: the returned AbstractTermDto still needs to use the case-sensitive version and actual transcript text is still case-sensitive~~

================================================================================================

============================================= 11/15/21 =========================================

FROM SATURDAY:
5. ~~Figure out how the client will request & update UserTerm details~~

TODO:
    
1. ~~Make sure that Terms/UserTerms are stored as normalized strings and retrieved based on the normalized value of the request string~~

    ~~-NOTE: the returned AbstractTermDto still needs to use the case-sensitive version and actual transcript text is still case-sensitive ~~
2. ~~Create Contents in Seed.cs~~
3. ~~UserTerm extension methods to do SRS algorithms (but do this first pussy ^^^)~~
4. ~~Migrate database and test the new term normalization/chunk creation stuff~~

~~PLANS/THOUGHTS ON TRANSCRIPT CHUNK HANDLING:~~
~~1. Split up chunk by splitting at whitespace~~
~~2. From each string, use regex to get the valid word characters, normalize and get AbstractTermDto from API~~
~~3. Remove the new string from the (normalized) original segment~~
~~4. If any characters are left over, add them as trailing characters~~

FOR LATER:
1. ~~Figure out how trailing characters can be displayed as components in React (i.e. part of the term component or its own component?)~~
2. ~~Figure out how to lay out react components like text in a paragraph~~
3. ~~Maybe devise a better planning/note-taking scheme than just typing shit on this markdown file~~

================================================================================================

============================================= 11/16/21 =========================================

FROM YESTERDAY:

1. ~~Figure out how trailing characters can be displayed as components in React- should be its own component~~
2. ~~Figure out how to lay out react components like text in a paragraph (i.e. learn abt CSS FlexBox/Semantic UI theming)~~
2. ~~Create Contents in Seed.cs~~
3. ~~UserTerm extension methods to do SRS algorithms (but do this first pussy ^^^)~~
4. ~~Migrate database and test the new term normalization/chunk creation stuff~~

TODO:

1. ~~Look into doing stuff with Bootstrap?~~


================================================================================================

============================================= 11/17/21 =========================================

FROM YESTERDAY:
1. ~~Figure out how trailing characters can be displayed as components in React- should be its own component~~
2. ~~Figure out how to lay out react components like text in a paragraph (i.e. learn abt CSS FlexBox/Semantic UI theming)~~
3. ~~Look into doing stuff with Bootstrap?~~

TODO:
1. ~~Figure out margins and other styling for TranscriptTerm component (also: figure out which semantic class/html tag to use)~~
2. ~~Dictionary API? (probably Yandex?)- low priority~~
3. ~~SelectedTerm component (to display translations, rating, and whatever else)~~
4. ~~Fix refreshing of content page to make sure the content in the URL is loaded and displayed~~

================================================================================================

============================================= 11/18/21 =========================================

FROM YESTERDAY:
2. Dictionary API? (probably Yandex?)- low priority
~~4. Fix refreshing of content page to make sure the content in the URL is loaded and displayed~~

TODO:
~~1. Fix selected word misalignment on client side~~
    ~~- Replace currentUserTerms array w/ a map indexed by indexInChunk~~
~~3. New seed with more contents~~

================================================================================================

============================================= 11/28/21 =========================================

FROM 11/18:
    2. Dictionary API? (back burner);

TODO:
   ~~1. Fix refershing on term creation such that:~~
       ~~1. The user doesn't need to click on the word again to have the refreshed value displayed~~
       ~~2. Every instance of the word in the chunk gets updated as well~~
   ~~3. Add indeces to TranscriptChunkDto~~
   ~~2. Make sure paging through content works~~
   ~~4. Add rating changer/srs updater for UserTerm on client side~~

================================================================================================
============================================= 11/29/21 =========================================

FROM YESTERDAY:

2. ~~Dictionary API? (back burner);~~

TODO:

1. ~~Add ChunkIndex to content route- Client~~
2. Vocab size/data handling- Server - Add "KnownWords" column to UserLanguageProfile entity
    NOTE: Figure out when & how this number should be updated - (have it check to increment/decrement every time the Rating is changed?)
3. ~~Expand 'Content' functionality:~~
    a. ~~Add ContentTag to content entity -Server~~
    b. ~~Show content header data as it relates to user- e.g percentage/number of words known~~
        ~~This needs to:~~
        1. ~~given a contentId and a username, return the percentage of words which have UserTerms~~
    c. ~~Give UserLanguageProfile a ContentHistory related entity -Server~~
4. ~~Figure out why GetKnownWordsForContent is so slow - research .NET performance bottlenecks/threading (or else is this just a fact of SQLite?)~~
5. ~~Make client-side display of known words on content headers (add knownWords to content header, just return an empty div)~~
6. Some sort of interface to get a graph of daily progress/statistics - Initialize with a list of ContentViewRecord queried by Date and LanguageProfileId
7. ~~ContentParser- class to parse URLs into 'ContentCreateDto'~~
8. ~~User Profile Page on client side~~
9. Client side display for profile details
10. Sort out regex stuff for cleaning gunk out of parsed HTML
=================================================================================================

============================================= 11/30/21 =========================================

FROM YESTERDAY:

2. ~~Client side display for profile details- low priority~~

TODO:
5. ~~In a new branch- refactor everything such that:~~

    ~~-No more Transcript entity~~
    
    ~~-Content only stores URL and groups of AbstractTerm are returned per paragraph of the parsed HTML~~

    ~~-Content extension methods to 1. Query the number of paragraphs 2. Return a Paragraph string when queried with an index~~

NOTES:

1. The HtmlContentParser should parse into ContentCreateDto as well as Access paragraphs and other HTML data

Parse Endpoints Needed:
1. Get ContentCreateDto from URL
2. ~~Get number of paragraphs from URL~~
=================================================================================================
============================================== 12/1/21 =========================================

FROM YESTERDAY:

1. Some sort of interface to get a graph of daily progress/statistics - Once backend is ready
2. ~~Sort out regex stuff for cleaning gunk out of parsed HTML~~
3. Figure out how to display/store header elements in parsed HTML pages - Once backend is ready

TODO:

4. "Import" enpoint for ContentController, should automatically parse and create content with params: URL 
5. ~~"AbstractTermsForParagraph" endpoint for ContentController (replaces AbstractTermsForChunk) with params: ContentUrl, Index~~
6. Figure out a way of combining really short "p" elements into a longer "ContentParagraph", update GetParagraphCount and GetParagraph to match
    -note: the logic for counting paragraphs and creating ContentParagraph objects should happen in the relevant HtmlContentParser subclass
7. Issue with separating trailing characters in GetAbstractTerm


NOTES:
   ~~ - AbstractTermsForParagraph: takes URL and Index: (note: this one needs all 3 services)~~
1. ~~Load the paragraph from the IParserService~~
2. ~~Split the paragraph into a list of TermIds~~
3. ~~Use IUserAccessor + DbContext extensions to create each AbstractTerm~~
=================================================================================================

============================================== 12/2/21 ==========================================
Merge NOTRANSCRIPTENTITY into master

FROM YESTERDAY:

1. ~~Some sort of interface to get a graph of daily progress/statistics - Once backend is ready~~ (table this for now)
3. Figure out how to display/store header elements in parsed HTML pages - Once backend is ready
4. ~~"Import" enpoint for ContentController, should automatically parse and create content with params: URL ~~
6. ~~Figure out a way of combining really short "p" elements into a longer "ContentParagraph", update GetParagraphCount and GetParagraph to match~~ (table)
7. Issue with separating trailing characters in GetAbstractTerm

TODO:

1. ~~Clean up unused endpoints~~
2. Refactor client side for new endpoints
=================================================================================================
============================================== 12/3/21 ==========================================

FROM YESTERDAY:

3. Figure out how to display/store header elements in parsed HTML pages - Once backend is ready
1. Clean up unused endpoints
2. Refactor client side for new endpoints

TODO (client refactoring):
1. Refactor functionality from transctiptStore.ts into existing stores
2. Fix components to match
=================================================================================================
============================================== 12/4/21 ==========================================

FROM YESTERDAY:

3. ~~Figure out how to display/store header elements in parsed HTML pages - Once backend is ready~~
1. Clean up unused endpoints
2. ~~Refactor client side for new endpoints~~

TODO (client refactoring):
1. ~~Refactor functionality from transctiptStore.ts into existing stores~~
2. ~~Fix components to match~~
3. ~~Make ContentWithName endpoint~~
4. ~~Ensure that the TranscriptPage automatically loads from the route URL (do a useEffect probably)~~
5. Fix refreshing of userTerms when they are created/ edited
6. Fix known words endpoint and client
=================================================================================================
============================================== 12/5/21 ==========================================

FROM YESTERDAY:

1. ~~Clean up unused endpoints~~
5. ~~Fix refreshing of userTerms when they are created/ edited~~
6. Fix known words endpoint and client

TODO :

1. ~~Fix updating of selectedTerm upon creation~~
2. ~~Add some wikipedia contents to Seed.cs and rebuild Db~~
3. Fix the "no valid characters" trailing characters thing
4. Write algo for grouping paragraphs into a more reasonable length-  remember getParagraphCount has to reflect this
5. ~~Add optional "paragraphHeader" property to ContentParagraph~~
6. ~~Add some delete endpoints (top of agenda)~~
7. ~~Fix issue when updating terms without changing anything~~
8. ~~Selecting a term needs to make a call for a list of translations~~
9. Add delete translation functionality
NOTES:

1. To kill 4 & 5 with one stone, define a paragraph as every 'p' element that lies between two headers
=================================================================================================
============================================== 12/6/21 ==========================================

FROM YESTERDAY:

3. Fix the "no valid characters" trailing characters thing
4. Write algo for grouping paragraphs into a more reasonable length-  remember getParagraphCount has to reflect this
9. ~~Add delete translation functionality~~

TODO:

1. ~~Add delete translation functionality to client~~
2. Figure out a way to calculate known words progressively or otherwise faster in some way

    - Maybe have the client make a series of calls which return the known words/total words for a given paragraph so that some number is returned
     and displayed quickly while getting getting more accurate with each paragraph it calculates?
3. ~~Add ContentViewRecord from client as appropriate, write endpoints/extensions to display profile data over time (list of known word count/date objects for example)- also API endpoints for doing these things~~
4. Read up on LINQ stuff and threading to see if API calls & database access can be faster
5. Rework MediatR classes to be easier on the database (avoid loading related data as much as possible, etc)
6. Figure out refresh tokens
7. ~~Refactor Domain such that each UserLanguageProfile has a list of ContentHistory(one for each viewed content)~~
8. ~~Figure out what kind of graphs to use on the client side, let that inform the API code~~

NOTES:

User profile data points:

1. Known words over time
2. History of contents viewed and dates
3. Get all the UserTerms created on a certain date
4. Maybe add something for how often other users have used translations created by the user?

=================================================================================================
============================================== 12/7/21 ==========================================

FROM YESTERDAY:

2. Figure out a way to calculate known words progressively or otherwise faster in some way
    - Maybe have the client make a series of calls which return the known words/total words for a given paragraph so that some number is returned
     and displayed quickly while getting getting more accurate with each paragraph it calculates?

TODO:

1. Figure out Wiki HTML parsing
2. Figure out client-side section headers
=================================================================================================
