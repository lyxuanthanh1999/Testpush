import {StyleSheet} from 'react-native';
import AppDimensions from './AppDimensions';
const styles = StyleSheet.create(
    {
        container : {
            flex:1,
        },
        textBrand:{
            position:'absolute',
            top:50,
            bottom:0,
            right:0,
            left:0,
            alignContent:'center',
            alignItems:'center',
           height:AppDimensions.getHeight()*(50/100),
           width:AppDimensions.getWith(),
        },
        viewButtonRegisterAndButtonLogin:{
            position:'absolute',
            bottom:50,
            left:0,
            right:0,
            flexDirection:'row',
    
    
        },
        Image_Back_Ground:{
            flex:1,
            resizeMode: 'stretch',
            height:AppDimensions.getHeight(),
            width:AppDimensions.getWith(),
            justifyContent:'center',
        },
        ViewSigUp:{
            position:'absolute',
            top:50,
            bottom:0,
            right:0,
            left:0,
            marginLeft:20,
            marginRight:20,
            alignContent:'center',
            alignItems:'center',
           height:AppDimensions.getHeight()*(20/100),
           width:AppDimensions.getWith()-30,
        },
        ViewLogin:{
            position:'absolute',
            top:70,
            bottom:0,
            right:0,
            left:0,
            marginTop:30,
            marginLeft:20,
            marginRight:20,
            alignContent:'center',
            alignItems:'center',
           height:AppDimensions.getHeight()*(20/100),
           width:AppDimensions.getWith()-30,
        },
        ViewButton:{
            alignItems:'center',
            width:AppDimensions.getWith()*(50/100),
        },
        ViewTextInput:{
            marginBottom:25,
            paddingBottom:20,
            height:180,
        },
        ViewTextInput1:{
            marginBottom:25,
            paddingBottom:20,
            height:280,
        },
        ButtonRegister:{
            width:150,
            height:45,
            flex:1,
            borderRadius:5,
            alignItems:'center',
            justifyContent:'center',
            backgroundColor:'coral'
        },
        ButtonLogin:{
            width:150,
            height:45,
            flex:1,
            borderRadius:12,
            alignItems:'center',
            justifyContent:'center',
            backgroundColor:'#00ffff'
        },
        ButtonComplete:{
            width:300,
            height:50,
            borderRadius:25,
            alignItems:'center',
            justifyContent:'center',
            backgroundColor:'coral',
            color:'white'
        },
        container:{
            flex:1,
        },
        FontChuButton:{
            fontSize:19,
            color:'white',
        },
        FontChuThuongHieu1:{
            fontSize:70,
            fontWeight:'900',
        },
        FontChuThuongHieu2:{
            fontWeight:'700',
            fontSize:24,
        },
        ViewStyleTextInput:{
            height:70,
            width:300,
            marginBottom:20,
        },
        ViewStyleTextInputLogin:{
            height:70,
            width:300,
        },
        StyleTextInput:{
            borderColor:'white',
            color:'white',
            borderRadius:5,
            borderWidth:2,
        },
        StyleText:{
            fontSize:19,
            fontStyle:'normal',
        },
        StyleTextConnectFB:{
            fontSize:20,
            fontStyle:'normal',
            fontWeight:"bold",
            color:'gold',
            marginLeft:10,
        },
        containerHome_ItemFlatList_moTa:{
            position:'absolute',
            top:130,
            bottom:0,
            right:0,
            left:-50,
            alignContent:'center',
            alignItems:'center',
            justifyContent:'center',
            height:AppDimensions.getHeight()/5,
            width:AppDimensions.getWith(),
        },
        containerHome_ItemFlatList_Ten:{
            position:'absolute',
            top:50,
            bottom:0,
            right:0,
            left:-30,
            padding:60,
            height:AppDimensions.getHeight()/5,
            width:AppDimensions.getWith(),
        },
        container1:{
            flex:1,
            width:AppDimensions.getWith(),
        },
        Image_Back_Ground_ItemFlatList:{
            flex:1,
            resizeMode: 'stretch',
            height:250,
            // width:AppDimensions.getWith(),
            width:'100%',
            justifyContent:'center',
        },
        FontChuDen:{
            fontSize:22,
            color:'black',
            fontWeight:'bold'
        },
        FontChuXam:{
            fontSize:19,
            color:'gray',
            fontWeight:'bold'
        },
        styleItemCarousel:{
            height:AppDimensions.getHeight()/2.6,
            width:AppDimensions.getWith(),
            borderRadius:5,
        },
        styleTextDiscover:{
            fontWeight:'bold',
            fontSize:21,
        },
        ViewDiscover:{
            flex:1,alignItems:'center',
            justifyContent:'center',
            height:25,
            marginTop:25,
        },
        ViewFlatItemWelcome:{
            backgroundColor:'white',
            height:AppDimensions.getHeight()/2,
            width:200,
        },
        ViewFlatItemWommens:{
            backgroundColor:'white',
            height:AppDimensions.getHeight()/2.5,
            width:AppDimensions.getWith()/2,
            marginTop:20,
            borderWidth:2,
            width:200,
        },
        ImageFlatItem_Welcome:{
            height:220
        },
        TextTenFlatItem_Welcome:{
            marginTop:10,
            fontSize:18,
            fontWeight:'bold',
            color:'blue'
        },
        TextGiaFlatItem_Welcome:{
            marginTop:10,
            fontSize:18,
            color:'black'
        },
        styleTouchableOpacity:{
            marginHorizontal:10,
        },
    }
)

export default styles;