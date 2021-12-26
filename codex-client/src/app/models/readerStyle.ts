
export interface TermStyle {
    // TODO: put relevant css properties in here

}

export const classNameForTag = (tag: string) : string => {
    switch (tag) {
        case "p":
            return "codex-reader-p";
        case "h1":
            return "codex-reader-h1";
        case "h2":
            return "codex-reader-h2";
        case "h3":
            return "codex-reader-h3";
        default:
            return "codex-reader-p";
    }
}





export const getStyle = (tag: string): TermStyle => {

    return {

    }
}