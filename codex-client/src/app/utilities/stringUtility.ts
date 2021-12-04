

export function asTermValue(input: string)
{
    let exp = new RegExp('([^\p{P}^\s]+)');
    return input.replace(exp, '');
}