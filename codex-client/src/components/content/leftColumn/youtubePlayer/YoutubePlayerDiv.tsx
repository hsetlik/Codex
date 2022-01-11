import { observer } from "mobx-react-lite";
import { createRef, useState } from "react";
import ReactPlayer from "react-player/youtube";
import { Container } from "semantic-ui-react";
import { ElementAbstractTerms } from "../../../../app/models/content";
import { sectionMsRange } from "../../../../app/stores/contentStore";
import { useStore } from "../../../../app/stores/store";
import CaptionElementDiv from "./CaptionElementDiv";

export default observer(function YoutubePlayerDiv() {
    const {contentStore} = useStore();
    const {selectedContentUrl, highlightedElement, setHighlightedElement, elementAtMs, currentSection, loadSectionForMs, currentSectionTerms} = contentStore;
    const handleSeek = (seconds: number) => {
        const ms = (seconds * 1000);
        loadSectionForMs(ms, selectedContentUrl);
    }
    const playerRef = createRef<ReactPlayer>();
    const handleProgress = (state: {
        played: number;
        playedSeconds: number;
        loaded: number;
        loadedSeconds: number; 
    }) => {
        const playedMs = state.playedSeconds * 1000;
        let current = elementAtMs(playedMs);
        if (highlightedElement !== current) {
            setHighlightedElement(current);
            console.log(`${current.contentUrl} at seconds ${current.startMs || 0 / 1000}`);
        }
        const range = sectionMsRange(currentSection!);
        if (range.start > playedMs || range.end <= playedMs) {
           //TODO: load the correct section for playedMs- make sure to use buffer if possible and handle loading time if not 
           console.log(`Loading section at ${playedMs} ms`);
           loadSectionForMs(playedMs, selectedContentUrl);
        }
    }
    const [isPlaying, setIsPlaying] = useState(false);
    const handleCaptionJump = (terms: ElementAbstractTerms) =>
    {
        const element = currentSection?.textElements.find(e => e.elementText === terms.elementText)!;
        playerRef.current?.seekTo((element.startMs || 0 / 1000), "seconds");
        if(!isPlaying) {
            setIsPlaying(true);
        }
    }

    const handlePlay = () => setIsPlaying(true);
    const handlePause = () => setIsPlaying(false);
    return (
        <div>
           <ReactPlayer 
           url={selectedContentUrl} 
           controls={true}
           onSeek={handleSeek}
           onProgress={handleProgress}
           onPlay={handlePlay}
           onPause={handlePause}
           progressInterval={400}
           ref={playerRef}
           playing={isPlaying}
           light
           />
           <Container>
            <div>
                {currentSectionTerms.elementGroups.map(cpt => (
                    <CaptionElementDiv terms={cpt} key={cpt.elementText} isHighlighted={highlightedElement?.elementText === cpt.elementText} jumpFunction={() => {
                        handleCaptionJump(cpt);
                    }} />
                ))}
            </div>
           </Container>
        </div>
    )
})