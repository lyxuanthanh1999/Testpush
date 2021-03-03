import React, { Component } from 'react'
import { useEffect, useState } from 'react'
import {FlatList,Button, Text,ScrollView,TextInput,Dimensions, TouchableOpacity, View,Image, Alert, Keyboard, TouchableWithoutFeedback } from 'react-native'
import AppDimensions from '../helpers/AppDimensions';
import styleSheet from '../helpers/StyleSheets'

export default function Login () {
    const [txtEmail, settxtEmail] = useState('');
    const [txtPassword, settxtPassword] = useState('');
    const [url, seturl] = useState('https://localhost:44342/swagger',);
    const fDangNhap=()=>{
        // const {txtEmail,txtPassword}= this.state;
        if(txtEmail.length === 0 || txtPassword.length ===  0)
        {
            return alert('Mời bạn nhập thông tin đầy đủ');
        }
    }
        return (
            //  <TouchableWithoutFeedback  onPress={Keyboard.dismiss}>
                    <View style={styleSheet.container}>
                        <View style={{flexDirection:'column',alignItems:'center',height:Dimensions.get('screen').height,width:AppDimensions.getWith()}}>
                            <View style={{height:'30%',width:'100%',justifyContent:'center'}}>
                                <Image source={require('../Image/logo3.jpg')} style={{height:'100%',width:'100%'}}/>
                            </View>
                            <View style={{backgroundColor:'black',height:'70%',width:'100%',alignItems:'stretch',paddingTop:50}}>
                                <View style={{flexDirection:'column'}}>
                                    <View style={{alignItems:'center',justifyContent:'center',}}>
                                        <Text style={{fontSize:30,fontWeight:'700',color:'white'}}> Đăng Nhập </Text>
                                    </View>
                                    <View style={{paddingTop:10}}>
                                        <Text style={{color:'white',fontWeight:'bold'}}>Tên Đăng Nhập</Text>
                                        <TextInput onChangeText={(text)=>{settxtEmail(text)}} textContentType='emailAddress' keyboardType='email-address' style={styleSheet.StyleTextInput}  placeholderTextColor='white' placeholder='Tên Đăng Nhập'></TextInput>
                                    </View>
                                    <View style={{paddingTop:10}}>
                                        <Text style={{color:'white',fontWeight:'bold'}}>Mật Khẩu</Text>
                                        <TextInput onChangeText={(text)=>{settxtPassword(text)}} secureTextEntry={true} style={styleSheet.StyleTextInput}  placeholderTextColor='white' placeholder='Mật Khẩu'></TextInput>    
                                    </View>
                                    <View style={{paddingTop:25,justifyContent:'center',alignItems:'center'}}>
                                        <TouchableOpacity style={styleSheet.ButtonComplete} onPress={()=>{fDangNhap()}}>
                                                <Text style={{fontSize:19,fontWeight:'bold',color:'white'}}>Đăng Nhập</Text>
                                        </TouchableOpacity>
                                    </View>
                                    <View style={{paddingTop:25,justifyContent:'center',alignItems:'center'}}>
                                        <Text style={styleSheet.StyleTextConnectFB} onPress={()=>{alert('Bấm vào quên mật khẩu')}}>Quên Mật Khẩu </Text>
                                    </View>
                                    <View style={{paddingTop:25,justifyContent:'center',alignItems:'center'}}>
                                        <Text style={styleSheet.StyleTextConnectFB} onPress={()=>{alert('Bấm vào Đăng Ký')}}>Đăng Ký </Text>
                                    </View>
                                </View>
                            </View>
                        </View>
                        {/* <View style={{alignItems:'center',height:200,width:AppDimensions.getWith()}}>
                            <Image source={require('../Image/logo1.png')} style={{height:120,resizeMode:'contain',width:350}}/>
                        </View>
                        <View style={styleSheet.ViewLogin}>
                            <View style={{margin:20}}>
                                <Text style={{fontSize:30,fontWeight:'700'}}> Đăng Nhập </Text>
                            </View>
                            <View style={styleSheet.ViewTextInput}>
                                <View style={styleSheet.ViewStyleTextInput}>
                                    <Text>Your Email</Text>
                                    <TextInput onChangeText={(text)=>{this.state.txtEmail = text}} textContentType='emailAddress' keyboardType='email-address' style={styleSheet.StyleTextInput}  placeholderTextColor='black' placeholder='Email address'></TextInput>
                                </View>
                                <View style={styleSheet.ViewStyleTextInput}>
                                    <Text>YourPassword</Text>
                                    <TextInput onChangeText={(text)=>{this.state.txtEmail = text}} secureTextEntry={true} style={styleSheet.StyleTextInput}  placeholderTextColor='black' placeholder='Password'></TextInput>
                                </View>
                            </View>
                            <View style={{padding:20}}>
                                <TouchableOpacity style={styleSheet.ButtonComplete} onPress={()=>{this.fDangNhap()}}>
                                        <Text style={{fontSize:19,}}>Đăng Nhập</Text>
                                </TouchableOpacity>
                            </View>
                            <View style={{height:70,width:300,alignContent:'center',justifyContent:'center',flexDirection:'row'}}>
                                <Text style={styleSheet.StyleTextConnectFB} onPress={()=>{alert('Bấm vào quên mật khẩu')}}>Quên Mật Khẩu </Text>
                            </View>
                            <View style={{height:70,width:300,alignContent:'center',justifyContent:'center',flexDirection:'row'}}>
                                <Text style={styleSheet.StyleTextConnectFB} onPress={()=>{alert('Bấm vào Đăng Ký')}}>Đăng Ký </Text>
                            </View>
                        </View>  */}
                </View>
    //  </TouchableWithoutFeedback>
        )
    }


