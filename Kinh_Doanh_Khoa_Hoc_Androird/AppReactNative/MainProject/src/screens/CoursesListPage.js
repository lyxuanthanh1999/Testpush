import React, { Component } from 'react'
import { useEffect, useState } from 'react'
import CoursesList from '../compoments/CoursesList'

export default function CoursesListPage ({navigation}) {
        return (
            <CoursesList navigation={navigation}/>
        )
    }

