import React, { Component } from 'react'
import { Text, View,FlatList } from 'react-native'

import { connect } from 'react-redux';
import CourseItem from '../compoments/CourseItem'

import ActionCreator from '../redux/Action/ActionCreator';

class Courses extends Component {
    constructor(props)
    {
        super(props);
        // console.log('constructor')
        // this.state = {
        //     courses : [
        //         {ID : 1,Name:'docker cơ bản',Image : require('../Image/docker-co-ban_m_1561455294.jpg'),Cost:699000,creationTime:'2020-02-12T00:00:00',discountPercent: 50},
        //         {ID : 2,Name:'lập trình android từ cơ bản',Image :require( '../Image/lap-trinh-android-tu-co-ban-den-thanh-thao_m.png'),Cost:699000,creationTime:'2020-02-12T00:00:00',discountPercent: 50},
        //         {ID : 3,Name:'lập trình kotlin toàn tập',Image :require('../Image/lap-trinh-kotlin-toan-tap_m_1555658121.jpg'),Cost:699000,creationTime:'2020-02-12T00:00:00',discountPercent: 50},
        //         {ID : 4,Name:'react native',Image :require('../Image/react-native.png'),Cost:699000,creationTime:'2020-02-12T00:00:00',discountPercent: 50},
        //     ]
        // }
    }
    componentDidMount(){
        this.props.fetchCourses();
        // console.log(this.props.fetchUser('user0'))
    }
    render() {
        return (
            <View style={{flex: 1, justifyContent: 'center'}}>
                <FlatList
                horizontal={true}
                data={this.props.courses1}
                keyExtractor={(item, index) => item.id.toString()}
                renderItem={({item,index}) => (
                    <CourseItem
                    item={item}
                    index={index}
                    navigation={this.props.navigation}
                    />
                )}
                />  
            </View>
        )
    }
}
const mapStateToProps = (state)=>{
    return {courses1: state.courses, coursesId: state.CoursesId}
}
export default connect(mapStateToProps,ActionCreator)(Courses);