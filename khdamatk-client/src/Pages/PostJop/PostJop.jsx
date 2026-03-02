 
export default function PostJop() {
  return (
    <div className="bg-gray-50 pb-3 min-h-screen ">

      {/* Black Banner */}
      <div className="bg-black text-white text-center p-10 text-3xl font-medium">
        Post your jobs for free to receive professional proposals Now!
      </div>

      {/* Card Wrapper */}
      <div className="max-w-5xl mx-auto -mt-8  mb-20 px-6">
        <div className="bg-white rounded-lg shadow-md p-12">

          <h2 className="text-2xl font-semibold mb-12">
            Post Your Job
          </h2>

          {/* Row 1 */}
          <div className="grid md:grid-cols-2 gap-14 mb-10">
            <div>
              <label className="block mb-3 text-lg">Service Category*</label>
              <select className="w-full border-b border-gray-400 py-3 outline-none bg-transparent">
                <option></option>
              </select>
            </div>

            <div>
              <label className="block mb-3 text-lg">Job Title*</label>
              <input
                type="text"
                className="w-full border-b border-gray-400 py-3 outline-none bg-transparent"
              />
            </div>
          </div>

          {/* Description */}
          <div className="mb-12">
            <textarea
              placeholder="description"
              rows="6"
              className="w-full bg-gray-200 rounded-md p-8 resize-none outline-none"
            ></textarea>
          </div>

          {/* Skills */}
          <div className="mb-10">
            <label className="block mb-3 text-lg">Skills</label>
            <input
              type="text"
              className="w-full border-b border-gray-400 py-3 outline-none bg-transparent"
            />
          </div>

          {/* Budget */}
          <div className="grid md:grid-cols-2 gap-14 mb-10">
            <div>
              <label className="block mb-3 text-lg">Budget To *</label>
              <select className="w-full border-b border-gray-400 py-3 outline-none bg-transparent">
                <option></option>
              </select>
            </div>

            <div>
              <label className="block mb-3 text-lg">Budget From *</label>
              <select className="w-full border-b border-gray-400 py-3 outline-none bg-transparent">
                <option></option>
              </select>
            </div>
          </div>

          {/* Duration */}
          <div className="grid md:grid-cols-2 gap-14 mb-10">
            <div>
              <label className="block mb-3 text-lg">Durtion *</label>
              <select className="w-full border-b border-gray-400 py-3 outline-none bg-transparent">
                <option></option>
              </select>
            </div>

            <div>
              <label className="block mb-3 text-lg">Time Commitment *</label>
              <select className="w-full border-b border-gray-400 py-3 outline-none bg-transparent">
                <option></option>
              </select>
            </div>
          </div>

          {/* Experience & Deadline */}
          <div className="grid md:grid-cols-2 gap-14 mb-14">
            <div>
              <label className="block mb-3 text-lg">Experience *</label>
              <select className="w-full border-b border-gray-400 py-3 outline-none bg-transparent">
                <option></option>
              </select>
            </div>

            <div>
              <label className="block mb-3 text-lg">job Deadline</label>
              <div className="flex items-center border-b border-gray-400 py-3">
                <input
                  type="date"
                  className="w-full outline-none bg-transparent"
                />
                {/* <FaCalendarAlt className="text-gray-500" /> */}
              </div>
            </div>
          </div>

          {/* Button */}
          <button className="flex items-center gap-3   from-purple-600 to-purple-800 text-white px-10 py-4 rounded-lg text-lg font-semibold hover:opacity-90 transition">
            {/* <FaPaperPlane /> */}
            Post
          </button>

        </div>
      </div>
    </div>
  );
}


 
