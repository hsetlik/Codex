import Flags from 'country-flag-icons/react/1x1';



const getFlagComponent = (lang: string, className: string) => {
    switch(lang) {
        case 'fr': {
            return (
                <Flags.FR className={className}  />
            )
        }
        case 'en': {
            return (
                <Flags.GB className={className}/>
            )
        }
        case 'ar': {
            return (
                <Flags.SA className={className}/>
            )
        }
        case 'es': {
            return (
                <Flags.ES className={className}/>
            )
        }
        case 'de': {
            return (
                <Flags.DE className={className}/>
            )
        }
        case 'zh': {
            return (
                <Flags.CN className={className}/>
            )
        }
        case 'hi': {
            return (
                <Flags.IN className={className}/>
            )
        }
        case 'bn': {
                return (
                    <Flags.IN className={className}/>
                )
            }
        case 'ja': {
            return (
                <Flags.JP className={className}/>
            )
        }
        case 'tr': {
            return (
                <Flags.TR className={className}/>
            )
        }
        case 'ru': {
            return (
                <Flags.RU className={className}/>
            )
        }
        default: {
            return (
                <Flags.US className={className}/>
            )
        }
    }
}


interface Props {
    lang: string,
    className?: string
}


export default function LanguageFlag1x1({lang, className}: Props) {
    return (
        <>
        {getFlagComponent(lang, className || 'div')}
        </>
    )
}

