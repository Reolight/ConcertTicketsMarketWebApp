import { Map, Placemark } from "@pbe/react-yandex-maps";

export default function YndMaps(props){
    return <Map
        defaultState={props.defaultState}
        {...props.rest}
    >
        {!!props.placemarks && props.placemarks.map((placemark, index) => <Placemark key={index}
            {...placemark}/>) }
    </Map>
}