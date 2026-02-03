import { Link } from 'react-router-dom'
import NoteFound from '../../assets/Images/NotFound.png'
import WhyChooesUs from '../../Components/WhyChooesUs/WhyChooesUs'

export default function NotFound() {
  return (
     <>
     <div className="container text-center mb-11 bg-white">
      <div className='p-9 space-y-10'>
        <img className='size-96 object-cover mx-auto' src={NoteFound}/>
        <h1 className='font-bold text-4xl'>Oops! Page not found</h1>
        <Link to='/'>
        <button className='btn w-fit text-white hover:bg-purple-900 duration-300 transition'>
           Back to Home 
        </button>
        </Link>
      </div>
     </div>
      
     
     
     
     
     
     
     
     
     
     </>
  )
}
