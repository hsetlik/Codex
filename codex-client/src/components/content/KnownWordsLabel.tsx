import { observer } from "mobx-react-lite";
import { useEffect } from "react";
import { Label } from "semantic-ui-react";
import { CodexPallette, cssString, LerpColor } from "../../app/common/uiColors";
import { useStore } from "../../app/stores/store";

interface Props {
    contentId: string
}

export default observer(function KnownWordsLabel({contentId}: Props) {
    const {knownWordsStore: {difficulties: knownWords, loadKnownWordsFor}} = useStore();
    useEffect(() => {
        if (!knownWords.has(contentId)) {
            loadKnownWordsFor(contentId);
        }
    }, [knownWords, loadKnownWordsFor, contentId]);
    const knownWordsData = knownWords.get(contentId);
    const knownPercentage = () => {
        let ratio = (knownWordsData?.knownWords! / knownWordsData?.totalWords!);
        return Math.round(ratio * 100);
    }
    const color = LerpColor(CodexPallette.unknownColor, CodexPallette.knownColor, (knownPercentage() / 100));
    return (
        <Label style={{'background-color': cssString(color)}} content={(knownWords.has(contentId)) ? `${knownPercentage()}% of words known` : 'Loading...'} />
    )
})