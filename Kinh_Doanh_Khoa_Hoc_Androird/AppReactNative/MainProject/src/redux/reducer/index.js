import {combineReducers} from 'redux';
import categoryReducer from './categoryReducer';
import coursesReducer from './coursesReducer';
import userReducer from './userReducer';

const rootReducer = combineReducers({
    courses: coursesReducer,
    categories : categoryReducer,
    users : userReducer ,
});
export default rootReducer;