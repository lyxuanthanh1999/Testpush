import React, { Component } from 'react'
import { Text, View,FlatList } from 'react-native'
import CategoryItem from '../compoments/CategoryItem'
export default class Categories extends Component {
    constructor(props)
    {
        super(props);
        // console.log('constructor')
        this.state = {
            categories : [
                {ID : 1,Name:'Ngoại Ngữ',Image : require('../Image/nn.jpg'),sortOrder:1},
                {ID : 2,Name:'Marketing',Image :require( '../Image/Marketing.jpg'),sortOrder:2},
                {ID : 3,Name:'Tin Học Văn Phòng',Image :require('../Image/intro-tinhocvanphong.png'),sortOrder:3},
                {ID : 4,Name:'Thiết Kế',Image :require('../Image/thiet-ke-do-hoa-la-gi.png'),sortOrder:4},
                {ID : 5,Name:'Kinh Doanh - Khởi Nghiệp',Image :require('../Image/kdkn.jpeg'),sortOrder:5},
                {ID : 6,Name:'Phát Triển Cá Nhân ',Image :require('../Image/khoa-hoc-phat-trien-ca-nhan_2.png'),sortOrder:6},
                {ID : 7,Name:'Sales, Bán Hàng',Image :require('../Image/sales.jpg'),sortOrder:7},
                {ID : 8,Name:'Công Nghệ Thông Tin',Image :require('../Image/it.jpg'),sortOrder:8},
                {ID : 9,Name:'Sức Khỏe - Giới Tính',Image :require('../Image/skgt.jpg'),sortOrder:9},
                {ID : 10,Name:'Phong Cách Sống',Image :require('../Image/thanh-cong.jpg'),sortOrder:10},
                {ID : 11,Name:'Nuôi Dạy Con',Image :require('../Image/dc.jpg'),sortOrder:11},
                {ID : 12,Name:'Hôn Nhân và Gia Đình',Image :require('../Image/hngd.jpg'),sortOrder:12},
                {ID : 13,Name:'Nhiếp Ảnh, Dựng Phim',Image :require('../Image/unnamed.jpg'),sortOrder:13},
            ]
        }
    }
    // componentDidMount(){
    //     console.log(this.state.categories)
    // }
    render() {
        return (
            <View style={{flex: 1, justifyContent: 'center'}}>
                <FlatList
                horizontal={true}
                data={this.state.categories}
                keyExtractor={(item, index) => item.ID.toString()}
                renderItem={({item,index}) => (
                    <CategoryItem
                    item={item}
                    index={index}
                    />
                )}
                />  
            </View>
        )
    }
}
