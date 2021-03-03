import React, { Component } from 'react'
import { Text, View,StyleSheet,Dimensions, TouchableWithoutFeedback, ScrollView, TouchableOpacity,Image} from 'react-native'
import Video,{FilterType} from 'react-native-video';
import AppDimensions from '../helpers/AppDimensions';
import LessonList from '../compoments/LessonList';
import IP from '../redux/Action/ActionType'
import { connect } from 'react-redux';
import ActionCreator from '../redux/Action/ActionCreator';
import DetailCoursesItem1 from '../compoments/DetailCoursesItem1';
import { FlatList } from 'react-native-gesture-handler';
import Axios from 'axios';
//lớp này không dùng nữa
class DetailCoursesItem extends Component {
    constructor(props)
    {
        super(props);
        this.state={
            data:'',
            url:'http://192.168.1.4:4000/api/',
            URL:'http://192.168.1.4:4000'
        }
    }
    componentDidMount(){
        console.log(this.props.id);
        //this.props.fetchCoursesId(this.props.id);
        Axios.get(this.state.url+'Courses/'+this.props.id)
        .then((response) =>{
            //const data = response.data;
            this.setState({data: response.data})
        }).catch((Error)=>{
            console.log('Lỗi : '+Error)
        })
    }
    render() {
        return (
            <View style={{backgroundColor:'black',height:Dimensions.get('screen').height,width:AppDimensions.getWith(),flexDirection:'column',}}>
            <View style={{height:'25%',width:'100%'}}>
                <View style={{height:'100%',width:'100%'}}>
                   <Image source={{uri:this.state.URL+this.state.data.image}}   resizeMode="stretch"    style={styles.backgroundVideo} />
                </View>
            </View>
            <View style={{height:'75%',width:'100%'}}>
                <View style={{justifyContent:'center',alignItems:'center',paddingTop:5,}}>
                    {/* <Text style={{fontSize:25,letterSpacing:1,color:'red',fontWeight:'bold'}}>{this.state.data.price}</Text> */}
                    <Text style={{fontSize:20,fontWeight:'bold',color:'red',textDecorationLine:this.state.data.discountPercent>0?'line-through':'none',
                                                                display:this.state.data.discountPercent < 0 ? 'none':'flex'}}>
                        {this.state.data.price} Đồng
                    </Text> 
                    <Text style={{fontSize:20,fontWeight:'bold',color:'red'}}>
                        {this.state.data.price-(this.state.data.price*this.state.data.discountPercent/100)} Đồng
                    </Text>                    
                </View>
                <View style={{justifyContent:'center',alignItems:'center',paddingTop:5,}}>
                    <Text style={{fontSize:25,letterSpacing:1,color:'yellow',fontWeight:'bold'}}>Nội dung bài học</Text>
                </View>
                <View style={{justifyContent:'center',alignItems:'center',paddingTop:5,}}>
                    <Text style={{fontSize:15,letterSpacing:1,fontWeight:'bold',color:'white'}}>
                        {this.state.data.description}
                    </Text>
                </View>
                <View style={{justifyContent:'center',alignItems:'center',paddingTop:5,}}>
                    <Text style={{fontSize:25,letterSpacing:1,fontWeight:'bold',color:'white'}}>
                        Mục : {this.state.data.categoryName} 
                    </Text>
                </View>
                <View style={{justifyContent:'center',paddingTop:5,height:'60%',width:'100%'}}>
                <TouchableOpacity style={{alignItems:'center',paddingTop:5,paddingHorizontal:5,paddingVertical:5,width:'100%'}}>
                    <View style={{backgroundColor:'rgba(125, 222, 169, 0.9)',borderRadius:25,width:'100%',height:70,}}>
                        <View style={{width:'100%',height:'100%',alignItems:'center',justifyContent:'center'}}>
                            <Text style={{fontSize:15,fontWeight:'bold'}}>
                                Mua Khóa Học
                            </Text>
                        </View>
                    </View>
                </TouchableOpacity>
                </View>
            </View>
        </View>
            // <DetailCoursesItem1 item={this.props.coursesId}/>
        )
    }
}
var styles = StyleSheet.create({
    backgroundVideo: {
      height:'100%',
      width:'100%',
    },
  });
const mapStateToProps = (state)=>{
    return {coursesId: state.courseID}
}
export default connect(mapStateToProps,ActionCreator)(DetailCoursesItem);
