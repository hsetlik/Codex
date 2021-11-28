import Color from "color";
import { AbstractTerm } from "../models/userTerm";

function toCssColor(r: number, g: number, b: number) {
    return `rgb(${r}, ${g}, ${b})!important`;
}

function lerp(a: number, b: number, t: number)
{
    return a + ((b - a) * t);
}

export function getColorForTerm(term: AbstractTerm) {
    if (term.hasUserTerm) {
        const knownColor = [255, 255, 255];
        const unknownColor = [10, 170, 211];
        const t = term.rating / 6;
        const newR = lerp(unknownColor[0], knownColor[0], t);
        const newG = lerp(unknownColor[1], knownColor[1], t);
        const newB = lerp(unknownColor[2], knownColor[2], t);
        console.log(`New UserTerm color is: ${newR}, ${newG}, ${newB}`);
        return Color.rgb(newR, newG, newB);    
    }
    else if(!term.hasUserTerm) {
        return Color.rgb(200, 200, 200);
    }
}