import { AbstractTerm } from "../models/userTerm";

function lerp(a: number, b: number, t: number)
{
    return a + ((b - a) * t);
}

export function getColorForTerm(term: AbstractTerm): string {
    if (term.hasUserTerm) {
        const knownColor = [255, 255, 255];
        const unknownColor = [10, 170, 211];
        const t = term.rating / 6;
        const newR = lerp(unknownColor[0], knownColor[0], t);
        const newG = lerp(unknownColor[1], knownColor[1], t);
        const newB = lerp(unknownColor[2], knownColor[2], t);
        return `rgb(${newR}, ${newG}, ${newB})`;
    }
    else return 'rgb (255, 255, 255)';
}