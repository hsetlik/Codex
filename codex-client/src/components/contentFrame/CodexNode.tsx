import { DOMNode } from "html-react-parser";


interface Props {
    sourceNode: DOMNode
}

export default function CodexNode({sourceNode}: Props) {
    // return an empty div if this isn't a valid element
    if (!(sourceNode instanceof Element)) {
        return (
            <div></div>
        )
    }


}