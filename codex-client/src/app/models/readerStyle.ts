
export interface TermStyle {
    // TODO: put relevant css properties in here

}

export const classNameForTerm = (tag: string) : string => {
    switch (tag) {
        case "p":
            return "codex-term-p";
        case "h1":
            return "codex-term-h1";
        case "h2":
            return "codex-term-h2";
        case "h3":
            return "codex-term-h3";
        default:
            return "codex-term-p";
    }
}


//
export const classNameForElement = (tag: string): string => {
    switch (tag) {
        case "p":
            return "codex-element-p";
        case "h1":
            return "codex-element-h1";
        case "h2":
            return "codex-element-h2";
        case "h3":
            return "codex-element-h3";
        default:
            return "codex-element-p";
    }
}




