import React, { Component } from 'react'
import { Alert, Dimensions, Text, View,TouchableOpacity } from 'react-native'
import AppDimensions from '../helpers/AppDimensions'
import styleSheet from '../helpers/StyleSheets'
export default function SuccessPayPage () {
        return (
            <View style={{backgroundColor:'black'}}>
                <View style={{alignItems:'center',justifyContent:'center',height:Dimensions.get('screen').height}}>
                        <View style={{width:AppDimensions.getWith()}}>
                            <Text style={{color:'white',fontSize:20,fontWeight:'bold',paddingLeft:70}}>Đã Thanh Toán Thành Công</Text>
                        </View>
                        <View style={{width:AppDimensions.getWith(),paddingTop:50}}>
                            <View style={{paddingLeft:45}}>
                                <TouchableOpacity style={styleSheet.ButtonComplete} onPress={()=>{Alert.alert('Đã Nhấn')}}>
                                    <Text style={{fontSize:19,fontWeight:'bold',color:'white'}}>Quay Về Trang Chủ</Text>
                                </TouchableOpacity>
                            </View>
                        </View>
                </View>
            </View>
        )
    }
