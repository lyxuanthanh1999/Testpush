import React, { Component } from 'react'
import ListCourses from '../compoments/ListCourses'

import { useEffect, useState } from 'react'

export default function ListCoursesPage ({navigation}){
    const [Courses, setCourses] = useState([
        {id : 1,name:'Học Lập Trình C/C++ Từ A - Z',Description:'Trang bị cho bạn kỹ năng lập trình ngôn ngữ C/C++ từ cơ bản đến nâng cao, được minh họa thông qua các bài tập thực hành thực tế nhất về C/C++',Image:`'Images/hoc-lap-trinh-c-c-a-toi-z-duong-tich-dat_m_1555574622.jpg'`,Content:'Ngon Ngu Lap Trinh',Price:'499000'},
        {id : 2,name:'Học Lập Trình C/C++ Từ A - Z',Description:'Trang bị cho bạn kỹ năng lập trình ngôn ngữ C/C++ từ cơ bản đến nâng cao, được minh họa thông qua các bài tập thực hành thực tế nhất về C/C++',Image:`'Images/hoc-lap-trinh-c-c-a-toi-z-duong-tich-dat_m_1555574622.jpg'`,Content:'Ngon Ngu Lap Trinh',Price:'499000'},
        {id : 3,name:'Học Lập Trình C/C++ Từ A - Z',Description:'Trang bị cho bạn kỹ năng lập trình ngôn ngữ C/C++ từ cơ bản đến nâng cao, được minh họa thông qua các bài tập thực hành thực tế nhất về C/C++',Image:`'Images/hoc-lap-trinh-c-c-a-toi-z-duong-tich-dat_m_1555574622.jpg'`,Content:'Ngon Ngu Lap Trinh',Price:'499000'},
        {id : 4,name:'Học Lập Trình C/C++ Từ A - Z',Description:'Trang bị cho bạn kỹ năng lập trình ngôn ngữ C/C++ từ cơ bản đến nâng cao, được minh họa thông qua các bài tập thực hành thực tế nhất về C/C++',Image:`'Images/hoc-lap-trinh-c-c-a-toi-z-duong-tich-dat_m_1555574622.jpg'`,Content:'Ngon Ngu Lap Trinh',Price:'499000'},
        {id : 5,name:'Học Lập Trình C/C++ Từ A - Z',Description:'Trang bị cho bạn kỹ năng lập trình ngôn ngữ C/C++ từ cơ bản đến nâng cao, được minh họa thông qua các bài tập thực hành thực tế nhất về C/C++',Image:`'Images/hoc-lap-trinh-c-c-a-toi-z-duong-tich-dat_m_1555574622.jpg'`,Content:'Ngon Ngu Lap Trinh',Price:'499000'},
        {id : 6,name:'Học Lập Trình C/C++ Từ A - Z',Description:'Trang bị cho bạn kỹ năng lập trình ngôn ngữ C/C++ từ cơ bản đến nâng cao, được minh họa thông qua các bài tập thực hành thực tế nhất về C/C++',Image:`'Images/hoc-lap-trinh-c-c-a-toi-z-duong-tich-dat_m_1555574622.jpg'`,Content:'Ngon Ngu Lap Trinh',Price:'499000'},
    ])
    useEffect(() => {
        console.log('ListCoursesPage');
    })
        return (
            //  Courses={Courses}
            <ListCourses navigation={navigation}/>
        )
    }

