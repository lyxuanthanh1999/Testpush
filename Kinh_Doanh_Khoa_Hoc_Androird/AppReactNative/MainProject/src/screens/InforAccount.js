import React, { Component } from 'react'
import { Text, View,TouchableOpacity,Dimensions,TextInput } from 'react-native'
import AppDimensions from '../helpers/AppDimensions'
import styleSheet from '../helpers/StyleSheets'
import { useEffect, useState } from 'react'
import DatePicker from 'react-native-datepicker'
import { ScrollView } from 'react-native-gesture-handler'

export default function InforAccount () {
    
    const [date, setdate] = useState("2020-12-30",)
        return (
            <ScrollView style={{backgroundColor:'black',height:Dimensions.get('screen').height,width:AppDimensions.getWith()}}>
                <View style={{flexDirection:'column'}}>
                        <View style={{height:'10%',paddingTop:10,width:'100%',alignItems:'center',justifyContent:'center'}}>
                            <Text style={{color:'gold',fontSize:25,fontWeight:'bold'}}>Thông Tin Cơ Bản</Text>
                        </View>
                        <View style={{height:'90%',width:'100%',paddingTop:30,flexDirection:'column'}}>
                            <View style={{alignItems:'center',justifyContent:'center',}}>
                                        <Text style={{fontSize:20,fontWeight:'700',color:'white'}}> Tên </Text>
                            </View>
                            <View style={{marginTop:10,marginBottom:10,height:70,width:'100%',alignItems:'center',justifyContent:'center'}}>
                                <TextInput  style={{ borderColor:'white',
                                                    color:'white',
                                                    borderRadius:5,
                                                    borderWidth:2,
                                                    height:'100%',
                                                    width:'100%'
                                                    }}  placeholderTextColor='white' placeholder='Tên'><Text>Nguyen Van A</Text></TextInput>
                            </View>
                            <View style={{alignItems:'center',justifyContent:'center',}}>
                                        <Text style={{fontSize:20,fontWeight:'700',color:'white'}}> Email </Text>
                            </View>
                            <View style={{marginTop:10,marginBottom:10,height:70,width:'100%',alignItems:'center',justifyContent:'center'}}>
                                <TextInput  style={{ borderColor:'white',
                                                    color:'white',
                                                    borderRadius:5,
                                                    borderWidth:2,
                                                    height:'100%',
                                                    width:'100%'
                                                    }}  placeholderTextColor='white' placeholder='Tên'><Text>tyanh185@gmail.com</Text></TextInput>
                            </View>
                            <View style={{alignItems:'center',justifyContent:'center',}}>
                                        <Text style={{fontSize:20,fontWeight:'700',color:'white'}}> Ngày Sinh </Text>
                            </View>
                            <View style={{marginTop:10,marginBottom:10,height:70,width:'100%',alignItems:'center',justifyContent:'center'}}>
                            <DatePicker
                                                    style={{width: '100%'}}
                                                    date={date}
                                                    mode="date"
                                                    placeholder="select date"
                                                    format="YYYY-MM-DD"
                                                    minDate="1900-01-01"
                                                    maxDate="2100-01-01"
                                                    confirmBtnText="Confirm"
                                                    cancelBtnText="Cancel"
                                                    customStyles={{
                                                    dateIcon: {
                                                        position: 'absolute',
                                                        left: 0,
                                                        top: 4,
                                                        marginLeft: 0
                                                    },
                                                    dateInput: {
                                                        marginLeft: 36
                                                    }
                                                    // ... You can check the source to find the other keys.
                                                    }}
                                                    onDateChange={(date) => {setdate(date)}}
                                                />
                            </View>
                            <View style={{alignItems:'center',justifyContent:'center',}}>
                                        <Text style={{fontSize:20,fontWeight:'700',color:'white'}}> Số điện thoại </Text>
                            </View>
                            <View style={{marginTop:10,marginBottom:10,height:70,width:'100%',alignItems:'center',justifyContent:'center'}}>
                                <TextInput  style={{ borderColor:'white',
                                                    color:'white',
                                                    borderRadius:5,
                                                    borderWidth:2,
                                                    height:'100%',
                                                    width:'100%'
                                                    }} keyboardType='numeric' placeholderTextColor='white' placeholder='Tên'><Text>0926843110</Text></TextInput>
                            </View>
                        </View>
                </View>
            </ScrollView>
        )
}
