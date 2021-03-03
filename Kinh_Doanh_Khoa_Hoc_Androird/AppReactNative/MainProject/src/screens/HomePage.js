import React, { Component } from 'react'
import { Text, View,Dimensions, ScrollView,FlatList, TouchableOpacity } from 'react-native'
import AppDimensions from '../helpers/AppDimensions'
import Categories from '../compoments/Categories'
import Courses from '../compoments/Courses'
import { useEffect, useState } from 'react'

export default function HomePage ({navigation,route}) {
        return (
            <ScrollView style={{backgroundColor:'black'}}>
                <View>
                    <View style={{flexDirection:'column'}}>
                        <View style={{flex:1,flexDirection:'row'}}>
                            <View style={{justifyContent:'center',alignItems:'center'}}>
                                <Text style={{marginLeft:20,fontSize:30,fontWeight:'bold',color:'white'}}> Gợi ý </Text>
                            </View>
                        </View>
                        <View>
                            <Text style={{marginLeft:20,fontSize:17,color:'white'}}>Dành Cho Bạn</Text>
                        </View>
                    </View>
                    <View style={{justifyContent:'center',alignItems:'center',flexDirection:'row',height:Dimensions.get('screen').height/1.8,width:Dimensions.get('screen').width,}}>
                        <Courses navigation={navigation} />
                    </View>
                </View>
                <View style={{marginTop:20}}>
                    <View style={{flexDirection:'column'}}>
                        <View style={{flex:1,flexDirection:'row',width:AppDimensions.getWith()}}>
                            <View style={{justifyContent:'center',alignItems:'center',width:'50%'}}>
                                <Text style={{marginLeft:20,fontSize:30,fontWeight:'bold',color:'white'}}>Nhiều người quan tâm</Text>
                            </View>
                            {/* <View style={{marginLeft:30,paddingTop:20,width:'50%'}}>
                                <TouchableOpacity
                                 style={{height:50,width:150,borderColor:'white',borderRadius:25,borderWidth:2,justifyContent:'center',alignItems:'center'}}> 
                                    <Text style={{fontSize:20,color:'white'}}>Xem Thêm</Text>
                                </TouchableOpacity>
                            </View> */}
                        </View>
                        <View>
                            <Text style={{marginLeft:20,fontSize:17,color:'white'}}>20 khóa học được xem nhiều nhất</Text>
                        </View>
                    </View>
                    <View style={{justifyContent:'center',alignItems:'center',flexDirection:'row',height:Dimensions.get('screen').height/1.8,width:Dimensions.get('screen').width,}}>
                        <Courses />
                    </View>
                </View>
                
                {/* 
                <View>
                    <View style={{flexDirection:'column'}}>
                        <Text style={{marginLeft:20,fontSize:30,fontWeight:'bold',color:'black'}}> Mua nhiều nhất tuần </Text>
                        <Text style={{marginLeft:20,fontSize:17,color:'black'}}>20 khóa học được mua nhiều nhất tuần</Text>
                    </View>
                    <View style={{flexDirection:'row',height:Dimensions.get('screen').height/2,width:Dimensions.get('screen').width,borderWidth:2}}>
                            
                    </View>
                </View> */}
            </ScrollView>
        )
    }


    // <View style={{marginTop:20}}>
    //                 <View style={{flexDirection:'column'}}>
    //                     <View style={{flex:1,flexDirection:'row',width:AppDimensions.getWith()}}>
    //                         <View style={{justifyContent:'center',alignItems:'center',width:'50%'}}>
    //                             <Text style={{marginLeft:20,fontSize:30,fontWeight:'bold',color:'white'}}>Phổ biến nhất</Text>
    //                         </View>
    //                         <View style={{marginLeft:30,paddingTop:20,width:'50%'}}>
    //                             <TouchableOpacity style={{height:50,width:150,borderColor:'white',borderRadius:25,borderWidth:2,justifyContent:'center',alignItems:'center'}}> 
    //                                 <Text style={{fontSize:20,color:'white'}}>Xem Thêm</Text>
    //                             </TouchableOpacity>
    //                         </View>
    //                     </View>
    //                     <View>
    //                         <Text style={{marginLeft:20,fontSize:17,color:'white'}}>xu hướng học tập của thời đại</Text>
    //                     </View>
    //                 </View>
    //                 <View style={{justifyContent:'center',alignItems:'center',flexDirection:'row',height:Dimensions.get('screen').height/1.8,width:Dimensions.get('screen').width,}}>
    //                     <Courses />
    //                 </View>
    //             </View>
    //             <View>
    //             <View style={{flexDirection:'column',marginTop:20}}>
    //                     <View style={{flex:1,flexDirection:'row',width:AppDimensions.getWith()}}>
    //                         <View style={{justifyContent:'center',alignItems:'center'}}>
    //                             <Text style={{marginLeft:20,fontSize:30,fontWeight:'bold',color:'white'}}>Danh Mục Khóa Học</Text>
    //                         </View>
    //                     </View>
    //                 </View>
    //                 <View style={{justifyContent:'center',alignItems:'center',flexDirection:'row',height:Dimensions.get('screen').height/3,width:Dimensions.get('screen').width,}}>
    //                     <Categories />
    //                 </View>
    //             </View>