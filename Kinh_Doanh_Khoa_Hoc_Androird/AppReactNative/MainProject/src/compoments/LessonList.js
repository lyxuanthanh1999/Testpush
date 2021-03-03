import React, { Component } from 'react'
import { FlatList, Text, View } from 'react-native'
import LessonListItem from '../compoments/LessonListItem'
export default class LessonList extends Component {
    constructor(props)
    {
        super(props);
        this.state = {
            Lessons: [
                {ID : 1,Name:'Bài 0 Chia sẽ kinh nghiệm tìm hiểu code',VideoPath:'videos/Bai0_Chia_se_kinh_nghiem_tim_hieu_code.MP4',sortOrder:1},
                {ID : 2,Name:'Bài 1_1 Cấu hình sublimText cho React Native',VideoPath:'videos/Bai1_1_Cau_hinh_sublimText_Cho_React_Naitve_.MP4',sortOrder:2},
                {ID : 3,Name:'Bài 1_2 Cách Xuất log và gói code javascript',VideoPath:'videos/Bai1_2_cach_xuat log_va_goi_code_javascript.MP4',sortOrder:3},
                {ID : 4,Name:'Bài 1 Hướng Dẫn Cài Đặt React Native',VideoPath:'videos/Bai1_Huong dan_cai_dat_React_Native.MP4',sortOrder:4},
                {ID : 5,Name:'Bài 2 Biến Let Var Const và Template virals trong ES6',VideoPath:'videos/Bai2_Bien_Let_ Var_ Const_va_Template_Virals_trong_ES6.MP4',sortOrder:5},
                {ID : 6,Name:'Bài 3 Arrow Function ES6',VideoPath:'videos/Bai3_Arrow_Function ES6.MP4',sortOrder:6},
            
            ]
        }
    }
    render() {
        return (
            <View>
                <FlatList
                    data={this.state.Lessons}
                    keyExtractor={(item, index) => item.ID.toString()}
                    renderItem={({item,index}) => (
                    <LessonListItem
                    item={item}
                    index={index}
                    />
                )}
                />
            </View>
        )
    }
}
