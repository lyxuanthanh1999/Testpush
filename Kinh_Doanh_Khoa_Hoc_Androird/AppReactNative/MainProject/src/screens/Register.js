import React, { Component } from 'react'
import { useEffect, useState } from 'react'
import { Text,TextInput,ImageBackground, TouchableOpacity, View,Image, Alert, Keyboard, TouchableWithoutFeedback,Dimensions } from 'react-native'
import AppDimensions from '../helpers/AppDimensions';
import styleSheet from '../helpers/StyleSheets'
import DatePicker from 'react-native-datepicker'

export default function Register () {
    const [txtEmail, settxtEmail] = useState('');
    const [txtPassword, settxtPassword] = useState('');
    const [txtYourName, settxtYourName] = useState('')
    const [txtRePassword, settxtRePassword] = useState('')
    const [date, setdate] = useState("2020-12-30",)
    const  fKiemTraDinhDangEmail=(text)=>{
        let reg = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
        if (reg.test(text) === false) {
            console.log("Email is Not Correct");
            settxtEmail(text)
            return false;
        }
        settxtEmail(text)
        console.log("Email is Correct");
        return true;
    }
    const fDangKy=()=>{
        if(txtEmail.length === 0 || txtPassword.length ===  0 || txtYourName.length ===  0 || txtRePassword.length ===  0)
        {
            return alert('Mời bạn nhập thông tin đầy đủ');
        }
        if(txtPassword != txtRePassword)
        {
            return alert('Mật khẩu nhập lại không khớp');
        }
        if(!fKiemTraDinhDangEmail(txtEmail))
        {
            return alert('Sai Định Dạng Email');
        }
        
    }
        return (
            // <ImageBackground style={{flex:1,justifyContent:'center',resizeMode: "cover",}} source={require('../Image/t.jpg')}>
                // <TouchableWithoutFeedback  onPress={Keyboard.dismiss}>
                // </TouchableWithoutFeedback>           
            // </ImageBackground>
        <View style={styleSheet.container}>
                <View style={{flexDirection:'column',alignItems:'center',height:Dimensions.get('screen').height,width:AppDimensions.getWith()}}>
                            <View style={{height:'15%',width:'100%',justifyContent:'center'}}>
                                <Image source={require('../Image/logo3.jpg')} style={{height:'100%',width:'100%'}}/>
                            </View>
                            <View style={{backgroundColor:'black',height:'85%',width:'100%',alignItems:'stretch',paddingTop:50}}>
                                <View style={{flexDirection:'column'}}>
                                    <View style={{alignItems:'center',justifyContent:'center',}}>
                                        <Text style={{fontSize:30,fontWeight:'700',color:'white'}}> Đăng Ký </Text>
                                    </View>
                                    <View style={{paddingTop:10}}>
                                            <Text style={{color:'white',fontWeight:'bold'}}>Tên Của Bạn</Text>
                                            {/*  */}
                                            <TextInput 
                                            onChangeText={(text)=>{settxtYourName(text)}}
                                            textContentType='name' 
                                            keyboardType='default'
                                            autoFocus={true}
                                            style={styleSheet.StyleTextInput}
                                            placeholderTextColor='white'
                                            placeholder='Tên của bạn'></TextInput>
                                    </View>
                                    <View style={{paddingTop:10}}>
                                        <Text style={{color:'white',fontWeight:'bold'}}>Email Của Bạn</Text>
                                        <TextInput onChangeText={(text)=>{settxtEmail(text)}} textContentType='Địa chỉ email' keyboardType='email-address' style={styleSheet.StyleTextInput}  placeholderTextColor='white' placeholder='Email address'></TextInput>
                                    </View>
                                    <View style={{paddingTop:10}}>
                                        <Text style={{color:'white',fontWeight:'bold'}}>Mật Khẩu</Text>
                                        <TextInput onChangeText={(text)=>{settxtPassword(text)}} secureTextEntry={true} style={styleSheet.StyleTextInput}  placeholderTextColor='white' placeholder='Mật khẩu'></TextInput>    
                                    </View>
                                    <View style={{paddingTop:10}}>        
                                            <Text style={{color:'white',fontWeight:'bold'}}>Điền Lại Mật Khẩu</Text>
                                            <TextInput
                                            onChangeText={(text)=>{settxtRePassword(text)}}
                                            secureTextEntry={true}
                                            style={styleSheet.StyleTextInput}
                                            placeholderTextColor='white'
                                            placeholder='Điền lại mật khẩu'></TextInput>
                                    </View>
                                    <View>
                                            <Text style={{color:'white',fontWeight:'bold'}}>DOB</Text>
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
                                    <View style={{paddingTop:20,justifyContent:'center',alignItems:'center'}}>
                                        <TouchableOpacity style={styleSheet.ButtonComplete} onPress={()=>{fDangKy()}}>
                                                <Text style={{fontSize:19,fontWeight:'bold',color:'white'}}>Hoàn Thành Đăng Ký</Text>
                                        </TouchableOpacity>
                                        {/* <TouchableOpacity style={styleSheet.ButtonComplete} onPress={()=>{this.fDangNhap()}}>
                                                <Text style={{fontSize:19,fontWeight:'bold',color:'white'}}>Đăng Nhập</Text>
                                        </TouchableOpacity> */}
                                    </View>
                                    <View style={{paddingTop:15,justifyContent:'center',alignItems:'center'}}>
                                        <Text style={styleSheet.StyleTextConnectFB} onPress={()=>{alert('Đã Có Tài Khoản')}}>Đã Có Tài Khoản </Text>
                                    </View>
                                </View>
                            </View>
                        </View>
        </View>
        )
    }


