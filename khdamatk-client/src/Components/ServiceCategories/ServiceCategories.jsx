 
import ServiceCategoriesCard from '../ServiceCategoriesCard/ServiceCategoriesCard'
import Loading from '../Loading/Loading'
import { useEffect, useState } from 'react'
import { getHomeData } from '../../Services/home-api'
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faCircleInfo, faLayerGroup } from '@fortawesome/free-solid-svg-icons'

export default function ServiceCategories() {
    const [serviceCategories,setServiceCategories]=useState(null)
    const[loading,setLoading]=useState(true)
   async function getDataToServiceCategories(){
        try {
            const response= await getHomeData()
            if(response.isSuccess){
                setLoading(false)
                setServiceCategories(response.data.servicesCategories)
            }
        } catch (error) {
            setLoading(true)
            console.log(error)
            
        }
    }
    useEffect(()=>{
        getDataToServiceCategories()

    },[])
    if(loading){
        return <Loading/>

    }
  return (
//     <div className="container">
//            <section className="service-categories p-10 space-y-6">
//       <h2  className='font-bold text-3xl text-center'>Find Quality Freelancers & Remote Employees</h2>
//  <div className='flex items-center gap-2'>
    
//      {
//         serviceCategories.length  > 0 ? ( serviceCategories.map((category, index)=>(
//              <ServiceCategoriesCard key={index}
//               category={category}/>
//         ))) :(
//        <p className="mt-4 text-center text-xl mx-auto">
//   Categories will appear here once available
// </p>
 



 
//         )
//      }
//  </div>
//     </section>
//     </div>
<section className='dark:bg-gray-950 '>

  <div className="
  container
  service-categories 
  p-10 space-y-6
   dark:bg-gray-950
  transition-colors duration-300
  ">

    <h2 className="
    font-bold text-3xl text-center
    text-gray-900 dark:text-gray-100
    ">
      Find Quality Freelancers & Remote Employees
    </h2>

    <div className="
    flex flex-wrap justify-center gap-6
    ">

      {
        serviceCategories.length > 0 ? (
          serviceCategories.map((category, index) => (
            <ServiceCategoriesCard 
              key={index}
              category={category}
            />
          ))
        ) : (
          <p className="
          mt-4 text-center text-xl mx-auto
          text-gray-600 dark:text-gray-400
          ">
            Categories will appear here once available
          </p>
        )
      }

    </div>

  </div>

</section>
  )
}
