import { use, useState } from "react";
import { useNavigate } from "react-router-dom";
import { register } from "../services/authService";

const Register = () => {
  const [name, setName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [age, setAge] = useState("");
  const [weight, setWeight] = useState("");
  const [height, setHeight] = useState("");
  const [gender, setGender] = useState("");
  const [profileImage, setProfileImage] = useState(null);
  const [errorMessage, setErrorMessage] = useState("");
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
  
    const formData = new FormData();
    formData.append('name', name);
    formData.append('email', email);
    formData.append('password', password);
    formData.append('age', age);
    formData.append('weight', weight);
    formData.append('height', height);
    formData.append('gender', gender);
    formData.append('profileImage', profileImage); // Dosyayı ekleyin
  
    try {
      await register(formData); // FormData'yı gönderin
      navigate('/login');
    } catch (error) {
      setErrorMessage(error.message || 'Registration failed');
    }
  };

  return (
    <div>
      <h2>Register</h2>
      {errorMessage && <p style={{ color: "red" }}>{errorMessage}</p>}
      <form onSubmit={handleSubmit}>
        <div>
          <label>Profile Photo:</label>
          <input
            type="file"
            onChange={(e) => setProfileImage(e.target.files[0])}
            required
          />
        </div>
        <div>
          <label>Name:</label>
          <input
            type="text"
            value={name}
            onChange={(e) => setName(e.target.value)}
            required
          />
        </div>
        <div>
          <label>Email:</label>
          <input
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
        </div>
        <div>
          <label>Password:</label>
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
          <div>
            <label>Age:</label>
            <select
              value={age}
              onChange={(e) => setAge(e.target.value)}
              required
            >
              <option value="">Select Age</option>
              {Array.from({ length: 83 }, (_, i) => i + 18).map((ageOption) => (
                <option key={ageOption} value={ageOption}>
                  {ageOption}
                </option>
              ))}
            </select>
          </div>
          <div>
            <label>Weight (kg):</label>
            <input
              type="number"
              value={weight}
              onChange={(e) => setWeight(e.target.value)}
              required
              min="1"
              step="any"
            />
          </div>
          <div>
            <label>Height (cm):</label>
            <input
              type="number"
              value={height}
              onChange={(e) => setHeight(e.target.value)}
              required
              min="1"
            />
          </div>
          <div>
            <label>Gender:</label>
            <select
              value={gender}
              onChange={(e) => setGender(e.target.value)}
              required
            >
              <option value="">Select Gender</option>
              <option value="male">Male</option>
              <option value="female">Female</option>
            </select>
          </div>
        </div>
        <button type="submit">Register</button>
      </form>
    </div>
  );
};

export default Register;
