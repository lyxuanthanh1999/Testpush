import React, { Component } from 'react'
import { Text, View,StyleSheet,Dimensions, TouchableWithoutFeedback, ScrollView, TouchableOpacity,Image} from 'react-native'
import Video,{FilterType} from 'react-native-video';
import AppDimensions from '../helpers/AppDimensions';
import LessonList from '../compoments/LessonList';
import IP from '../redux/Action/ActionType'
// lớp này không dùng nữa
export default class DetailCoursesItem1 extends Component {
    constructor(props){
        super(props);

    }
    componentDidMount(){
        console.log('DetailCoursesItem1=>>'+JSON.stringify(this.props.item))
    }
    render() {
        return (
            <ScrollView style={{backgroundColor:'black',height:Dimensions.get('screen').height,width:AppDimensions.getWith(),flexDirection:'column',}}>
                <View style={{height:'25%',width:'100%'}}>
                    <View style={{height:'100%',width:'100%'}} onPress={()=>{this.setState({paused:!this.state.paused},console.log(this.state.paused))}}>
                       <Video source={require('../Image/a.mp4')}   // Can be a URL or a local file.
                                                         // onBuffer={this}
                                                         filter={FilterType.MAXIMUMCOMPONENT}
                                                         filterEnable={true}
                                                         controls={true}
                                                         paused={true}
                                                         muted ={false}
                                                         resizeMode="stretch"
                                                         style={styles.backgroundVideo} />
                    </View>
                </View>
                <View style={{height:'75%',width:'100%'}}>
                    <View style={{justifyContent:'center',alignItems:'center',paddingTop:5,}}>
                        <Text style={{fontSize:25,letterSpacing:1,color:'red',fontWeight:'bold'}}>Giá : 499000 Đồng</Text>
                    </View>
                    <View style={{justifyContent:'center',alignItems:'center',paddingTop:5,}}>
                        <Text style={{fontSize:25,letterSpacing:1,color:'yellow',fontWeight:'bold'}}>Nội dung bài học</Text>
                    </View>
                    <View style={{justifyContent:'center',alignItems:'center',paddingTop:5,}}>
                        <Text style={{fontSize:15,letterSpacing:1,fontWeight:'bold',color:'white'}}>
                            Nội dung khoá học lập trình app bao gồm:
                            - Tìm hiểu cơ bản về ngôn ngữ lập trình Java (Từ định nghĩa đến kiểu dữ liệu, biến, câu lệnh, mảng, chuỗi ... cấu trúc điều khiển trong java, phương thức, lớp....)
                            - Cung cấp những kiến thức cốt lõi cơ bản về 1 ứng dụng Android bằng cách nói về vòng đời của một ứng dụng
                            - Cung cấp kiến thức về thành phần cơ bản cấu thành nên ứng dụng Android (Activity + intent + content provider + service)
                            - Hướng dẫn thực hành tạo ứng dụng Android: ứng dụng quản lý, ứng dụng media, ứng dụng trên Google Play.

                            Lợi ích từ khoá học
                            Có được kiến thức toàn diện về ngôn ngữ lập trình Java.
                            Nắm rõ những kiến thức về lập trình ứng dụng Android từ cơ bản đến nâng cao.
                            Có khả năng tạo được những ứng dụng Android, ứng dụng game Android.
                            Phù hợp với
                            Sinh viên không học CNTT muốn tìm cơ hội việc làm khác trong lĩnh vực CNTT.
                            Những bạn có đam mê, yêu thích lĩnh vực lập trình ứng dụng Android.
                        </Text>
                    </View>
                    <View style={{justifyContent:'center',alignItems:'center',paddingTop:5,}}>
                        <Text style={{fontSize:25,letterSpacing:1,fontWeight:'bold',color:'white'}}>
                            Tên Giáo Viên 
                        </Text>
                    </View>
                    <View style={{justifyContent:'center',alignItems:'center',paddingTop:5,}}>
                        <Text style={{fontSize:20,letterSpacing:1,fontWeight:'bold',color:'white'}}>
                            Trần Duy Thanh
                        </Text>
                    </View>
                    <View style={{justifyContent:'center',alignItems:'center',paddingTop:5,}}>
                        <LessonList/>
                    </View>
                </View>
            </ScrollView>
        )
    }
}

var styles = StyleSheet.create({
    backgroundVideo: {
      height:'100%',
      width:'100%',
    },
  });