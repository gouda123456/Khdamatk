import photo from '../../assets/Images/Logo.png'

export default function TestimonialCard() {
  return (
     <div>
        <div className=" flex items-center justify-center mb-4">
            <div className="card w-72 text-center  border border-Purple-300 rounded-2xl p-6 shadow-sm hover:shadow-md transition-shadow duration-300 ">
               
                <div>
                      <img src={photo} className='size-14 object-cover rounded-full mx-auto mb-3'/>
                    <h2 className='text-lg font-semibold text-black'>Bichoy Asaad</h2>
                    <h3 className='text-sm text-gray-500'>Product Manager</h3>
                    <p className='text-sm text-black leading-relaxed mt-5'>I had a great experience with elharefa team, they are very professional, friendly and understand well the client's needs. If you are looking for high-level and talented freelancers just contact them and they will provide you with the best match.</p>
                </div>
            </div>
        </div>









     </div>
  )
}
