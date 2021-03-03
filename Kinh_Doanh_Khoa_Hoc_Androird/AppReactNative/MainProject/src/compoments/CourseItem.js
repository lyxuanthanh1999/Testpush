import React, {Component} from 'react';
import {Text, View, StyleSheet, TouchableOpacity,Modal,Image, ImageBackground} from 'react-native';
import AppDimensions from '../helpers/AppDimensions';
import IP from '../redux/Action/ActionType'

// 
export default class CourseItem extends Component {
    constructor(props) {
        super(props);
        this.state = {
          modalVisible: false,
        };
      }
    //   http://192.168.0.192:4000/
    componentDidMount(){
        
         //this.props.fetchCoursesId(1);
        // console.log('===>'+this.props.navigation)
    }
      renderItemCourse = (course,index,navigation) => {
        return (
            <View style={{flexDirection:'column',flex:1, borderRadius:25,width:AppDimensions.getWith(),marginRight:10,borderWidth:2,borderColor:'white',backgroundColor:index%2===0?'rgba(125, 222, 169, 0.9)':'rgba(204, 125, 222, 0.9)'}}>
                                <View style={{flexDirection:'column'}}>
                                    <Image  source={{uri:IP.IP+course.image}} style={{
                                    borderRadius:25,
                                    height:AppDimensions.getHeight()/1.5,width:AppDimensions.getWith(),
                                        resizeMode: "cover",
                                        justifyContent: "center"}}/>
                                </View>
                                <View style={{flex:1,flexDirection:'row',width:AppDimensions.getWith()}}>
                                        <View style={{width:'50%',justifyContent:'center',alignItems:'center'}}>
                                            <Text style={{fontSize:25,fontWeight:'bold',textTransform:'uppercase'}}>{course.name}</Text>
                                            <Text style={{fontSize:20,fontWeight:'bold',color:'red',textDecorationLine:course.discountPercent>0?'line-through':'none',display:course.discountPercent < 0 ? 'none':'flex'}}>
                                                    {course.price} Đồng
                                            </Text> 
                                            <Text style={{fontSize:20,fontWeight:'bold',color:'red'}}>
                                                    {course.price-(course.price*course.discountPercent/100)} Đồng
                                            </Text>
                                            {/* <Text style={{fontSize:20}}>
                                                {course.creationTime}
                                            </Text> */}
                                        </View>
                                        <View style={{width:'50%',justifyContent:'center',alignItems:'center',marginLeft:20}}>
                                            <TouchableOpacity
                                            onPress={()=>{navigation.navigate('DetailCourses',{ id:course.id })}}  
                                             style={{height:50,width:150,backgroundColor:'white',borderColor:'black',borderRadius:25,borderWidth:2,justifyContent:'center',alignItems:'center'}}> 
                                                <Text style={{fontSize:20,fontWeight:'900'}}>Xem Chi Tiết</Text>
                                            </TouchableOpacity>
                                        </View>
                                </View>
            </View> 
        );
      };
    render() {
        return this.renderItemCourse(this.props.item,this.props.index,this.props.navigation);
    }
}


 {/* <ImageBackground 
                //loi require
                                 source={course.Image}
                                style={{flex: 1,
                                    resizeMode: "cover",
                                    justifyContent: "center"
                                }}>
                    <Text style={{fontSize:30}}>{course.ID}</Text>
                    <Text>{course.Name}</Text>
                    <Text>{course.Cost}</Text>
                </ImageBackground> */}