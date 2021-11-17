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
2. Make sure that Terms/UserTerms are stored as normalized strings and retrieved based on the normalized value of the request string

    -NOTE: the returned AbstractTermDto still needs to use the case-sensitive version and actual transcript text is still case-sensitive 

================================================================================================

============================================= 11/15/21 =========================================

FROM SATURDAY:
5. ~~Figure out how the client will request & update UserTerm details~~

TODO:
    
1. ~~Make sure that Terms/UserTerms are stored as normalized strings and retrieved based on the normalized value of the request string~~

    ~~-NOTE: the returned AbstractTermDto still needs to use the case-sensitive version and actual transcript text is still case-sensitive ~~
2. Create Contents in Seed.cs
3. UserTerm extension methods to do SRS algorithms (but do this first pussy ^^^)
4. Migrate database and test the new term normalization/chunk creation stuff

~~PLANS/THOUGHTS ON TRANSCRIPT CHUNK HANDLING:~~
~~1. Split up chunk by splitting at whitespace~~
~~2. From each string, use regex to get the valid word characters, normalize and get AbstractTermDto from API~~
~~3. Remove the new string from the (normalized) original segment~~
~~4. If any characters are left over, add them as trailing characters~~

FOR LATER:
1. Figure out how trailing characters can be displayed as components in React (i.e. part of the term component or its own component?)
2. Figure out how to lay out react components like text in a paragraph
3. Maybe devise a better planning/note-taking scheme than just typing shit on this markdown file

================================================================================================

============================================= 11/16/21 =========================================

FROM YESTERDAY:

1. Figure out how trailing characters can be displayed as components in React- should be its own component
2. Figure out how to lay out react components like text in a paragraph (i.e. learn abt CSS FlexBox/Semantic UI theming)
2. ~~Create Contents in Seed.cs~~
3. ~~UserTerm extension methods to do SRS algorithms (but do this first pussy ^^^)~~
4. ~~Migrate database and test the new term normalization/chunk creation stuff~~

TODO:

1. Look into doing stuff with Bootstrap?


================================================================================================
