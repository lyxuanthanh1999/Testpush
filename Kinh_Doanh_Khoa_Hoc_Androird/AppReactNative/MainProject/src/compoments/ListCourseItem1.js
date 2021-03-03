import React, {Component} from 'react';
import {Text, View, StyleSheet, TouchableOpacity,Modal,Image, ImageBackground, Alert} from 'react-native';
import AppDimensions from '../helpers/AppDimensions';
import IP from '../redux/Action/ActionType'

export default class ListCourseItem1 extends Component {
    constructor(props) {
        super(props);
      }
      componentDidMount(){
        //    console.log('du lieu ListCourse Item : '+ JSON.stringify(this.props.item))
        //   console.log('=====>1'+this.props.navigation)
        //   console.log('renderItemListCoursePage ====>>2'+JSON.stringify(this.renderItemListCoursePage(this.props.item,this.props.index)));
        console.log(JSON.stringify(this.props.navigation));
      }
      renderItemListCoursePage = (Course,index) => {
          // console.log('trong hàm renderItemListCoursePage')
          // console.log('trong hàm renderItemListCoursePage =====> ' +JSON.stringify(Course))
        return (
          <TouchableOpacity onPress={()=>{this.props.navigation.navigate('DetailCourses',{id:Course.id})}} style={{paddingTop:5,paddingHorizontal:5,paddingVertical:5}}>
              <View style={{backgroundColor:index%2===0?'rgba(125, 222, 169, 0.9)':'rgba(204, 125, 222, 0.9)',borderRadius:25,width:'100%',height:200,alignItems:'center',justifyContent:'center'}}>
                  <View>
                  <Image  source={{uri:IP.IP+Course.image}} style={{
                                    borderRadius:25,
                                    height:'90%',width:AppDimensions.getWith(),
                                        resizeMode: "cover",
                                        justifyContent: "center"}}/>
                  </View>
                  <View style={{flexDirection:'column'}}>
                  <Text style={{fontSize:15,fontWeight:'bold'}}>
                      {Course.name}
                  </Text>
                  </View>
              </View>
          </TouchableOpacity>
        );
      };
    render() {
        return  this.renderItemListCoursePage(this.props.item,this.props.index);
    }
}
