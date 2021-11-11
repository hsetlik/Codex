
interface ILangName
{
    iso: string,
    fullName: string
}

export const IsoLangNames: ILangName[] = [
    {iso: "fr", fullName: "French"},
    {iso: "en", fullName: "English"},
    {iso: "ar", fullName: "Arabic"},
    {iso: "es", fullName: "Spanish"},
    {iso: "de", fullName: "German"},
    {iso: "zh", fullName: "Chinese"},
    {iso: "hi", fullName: "Hindi"},
    {iso: "bn", fullName: "Bengail"},
    {iso: "ja", fullName: "Japanese"},
    {iso: "tr", fullName: "Turkish"},
    {iso: "ru", fullName: "Russian"}
]

//return the full English name of a (supported) language based on its two-letter ISO 639-1 code
export function getLanguageName(iso: string) {
    for(var lang of IsoLangNames){
        if (lang.iso === iso) {
            return lang.fullName;
        } 
    }
    return "Language not found!"
}

export function getLanguageCode(name: string){
    for(var lang of IsoLangNames) {
        if (lang.fullName === name)
            return lang.iso;
    }
    return "No matching ISO code";
}