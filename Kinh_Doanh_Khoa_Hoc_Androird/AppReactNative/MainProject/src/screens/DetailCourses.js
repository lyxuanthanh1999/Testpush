
import React, { Component } from 'react'
import { useEffect, useState } from 'react'
import { Text, View,StyleSheet,Dimensions, TouchableWithoutFeedback, ScrollView, TouchableOpacity} from 'react-native'
import Video,{FilterType} from 'react-native-video';
import AppDimensions from '../helpers/AppDimensions';
import LessonList from '../compoments/LessonList';
import DetailCoursesItem from '../compoments/DetailCoursesItem';
// import { connect } from 'react-redux';
// import ActionCreator from '../redux/Action/ActionCreator';
// const mapStateToProps = (state)=>{
//     return {coursesId: state.CoursesId}
// }
// export default connect(mapStateToProps,ActionCreator)(CourseItem);

export default function DetailCourses ({navigation,route}){
    // const [repeat, setrepeat] = useState(false)
    // const [rate, setrate] = useState(1)
    // const [volume, setvolume] = useState(1)
    // const [mute, setmute] = useState(false)
    // const [resizMode, setresizMode] = useState('contain')
    // const [duration, setduration] = useState(0.0)
    // const [paused, setpaused] = useState(true)
    // const [rareText, setrareText] = useState('1.0')
    // const [pausedText, setpausedText] = useState('Play')
    // const [hideControls, sethideControls] = useState(false)
    // const [controls, setcontrols] = useState(true)
    // const [filterType, setfilterType] = useState(FilterType.MAXIMUMCOMPONENT)
    const id = route.params.id;
    useEffect(() => {
        // console.log('dtCourses =>'+id);
        console.log('===>'+JSON.stringify(route.params))
    })
        return (
            <DetailCoursesItem id ={id}/>
        )
    }

