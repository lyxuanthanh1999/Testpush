import React, { Component } from 'react'
import { Text, View,Dimensions,TouchableOpacity } from 'react-native'
import AppDimensions from '../helpers/AppDimensions'

export default function Account ({navigation,route}) {
        return (
            <View style={{backgroundColor:'black',height:Dimensions.get('screen').height,width:AppDimensions.getWith()}}>
                <View style={{flexDirection:'column'}}>
                        <View style={{height:'30%',paddingTop:150,width:'100%',alignItems:'center',justifyContent:'center'}}>
                            <Text style={{color:'gold',fontSize:25,fontWeight:'bold'}}>Nguyễn Văn A</Text>
                        </View>
                        <View style={{height:'70%',width:'100%',paddingTop:30,flexDirection:'column'}}>
                            <TouchableOpacity onPress={()=>{navigation.navigate('InforAccount')}} style={{marginTop:10,marginBottom:10,height:70,width:'100%',borderRadius:25,alignItems:'center',justifyContent:'center',backgroundColor:'slategray'}}>
                                <Text style={{color:'white',fontSize:20,fontWeight:'bold'}}>Thông tin cơ bản</Text>
                            </TouchableOpacity>
                            {/* <View style={{marginTop:10,marginBottom:10,height:70,width:'100%',borderRadius:25,alignItems:'center',justifyContent:'center',backgroundColor:'slategray'}}>
                                <Text style={{color:'white',fontSize:20,fontWeight:'bold'}}>Nghành</Text>
                            </View> */}
                            <TouchableOpacity style={{marginTop:10,marginBottom:10,height:70,width:'100%',borderRadius:25,alignItems:'center',justifyContent:'center',backgroundColor:'slategray'}}>
                                <Text style={{color:'white',fontSize:20,fontWeight:'bold'}}>Kích hoạt khóa học</Text>
                            </TouchableOpacity>
                            <TouchableOpacity style={{marginTop:10,marginBottom:10,height:70,width:'100%',borderRadius:25,alignItems:'center',justifyContent:'center',backgroundColor:'slategray'}}>
                                <Text style={{color:'red',fontSize:20,fontWeight:'bold'}}>Đăng Xuất</Text>
                            </TouchableOpacity>
                        </View>
                </View>
            </View>
        )
    }

