
export const LangStrings = [
    ["fr",  "French"],
    ["en",  "English"],
    ["ar",  "Arabic"],
    ["es",  "Spanish"],
    ["de",  "German"],
    ["zh",  "Chinese"],
    ["ru",  "Russian"],
    ["hi",  "Hindi"],
    ["bn",  "Bengali"],
    ["ja",  "Japanese"],
]

export default function getLanguageName(iso: string)
{
    console.log("Checking ISO: " + iso);
    for(const pair in LangStrings)
    {
        if(pair[0] === iso){
            return pair[1];
        }
    }
    return "Language not found!"
}