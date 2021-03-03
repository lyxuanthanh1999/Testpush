import {Dimensions} from 'react-native'

function getWith(){
    return Dimensions.get('window').width;
}
function getHeight(){
    return Dimensions.get('window').width;
}

const AppDimensions = {getWith,getHeight};
export default AppDimensions;