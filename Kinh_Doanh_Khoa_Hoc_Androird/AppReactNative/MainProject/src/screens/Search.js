import React, { Component } from 'react'
import { Dimensions, Text, View,TouchableOpacity, Alert } from 'react-native'
import { FlatList, TextInput } from 'react-native-gesture-handler'
import Icon from 'react-native-vector-icons/Ionicons'
import styleSheet from '../helpers/StyleSheets'
export default class Search extends Component {
    render() {
        return (
            <View style={{flexDirection:'column',backgroundColor:'black'}}>
                <View style={{height:Dimensions.get('screen').height,width:Dimensions.get('screen').width,flexDirection:'column'}}>
                    <View style={{height:'20%',width:'100%',flexDirection:'row'}}>
                        <View style={{flex:1,height:100,width:'80%',}}>
                            <TextInput 
                                                textContentType='name' 
                                                keyboardType='default'
                                                autoFocus={true}
                                                placeholderTextColor='white'
                                                placeholder='Search'                        
                            style={{height:60,width:'100%',borderWidth:2,borderRadius:25,borderColor:'red',color:'white'}}/>
                        </View>
                        <View style={{height:100,width:'20%',}}>
                                            <TouchableOpacity style={{
                                                        width:'100%',
                                                        height:60,
                                                        borderRadius:25,
                                                        alignItems:'center',
                                                        justifyContent:'center',
                                                        backgroundColor:'white',
                                                        color:'white'
                                                        }} onPress={()=>{Alert.alert('')}}>
                                            <Icon name='search' size={30}/>    
                                            </TouchableOpacity>
                        </View>
                    </View>
                    <View style={{height:'80%'}}>
                        <FlatList
                            
                        />                                    
                    </View>
                </View>
            </View>
        )
    }
}
