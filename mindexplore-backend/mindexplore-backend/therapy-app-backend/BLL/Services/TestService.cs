using therapy_app_backend.BLL.Interfaces;
using therapy_app_backend.DAL.Entities;
using therapy_app_backend.DAL.Interfaces;

namespace therapy_app_backend.BLL.Services
{
    
    public class TestService : ITestService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TestService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Test> GetTestResultByPatientId(int id)
        {
            var test = await _unitOfWork.Tests.GetTestByPatientId(id);
            return test;
        }
        public double CalculateCompatibilityScore(Dictionary<string, string> options, Therapist therapist, List<int> specialties)
        {

            double score = 0.0;

            //preferintele pacientului
            string space = "";
            bool lgbt = false;
            bool relationship = false;
            bool religion = false;
            bool christianity = false;
            bool islam = false;
            bool judaism = false;
            bool isMemberOfLgbt = false;
            bool isReligious = false;
            bool isRroma = false;
            bool isHungarian = false;
            bool isFemale = false;
            bool isMale = false;
            bool isNotReligious = false;
            bool under35 = false;
            bool above55 = false;
            bool widow = false;
            bool nonReligion = false;
            string therapistGender = "";

            //patient preferences
            foreach (KeyValuePair<string, string> option in options)
            {
                string key = option.Key;
                string value = option.Value;

                if (option.Key == "Ai cerinte specifice in legatura cu terapeutul?")
                {
                   
                        if (value == "Sa fie femeie")
                        {
                            isFemale = true;
                            if (therapist.Gender != null && therapist.Gender.ToLower() == "female")
                                score += 2.0;
                            /*else if (therapist.Gender == null)
                                score += 0.1;*/
                        }
                        else if (value == "Sa fie barbat")
                        {
                            isMale = true;
                            if (therapist.Gender != null && therapist.Gender.ToLower() == "male")
                                score += 2.0;
                            /*else if (therapist.Gender == null)
                                score += 0.1;*/
                        }
                        else if (value == "Sa apartina comunitatii LGBTQ+")
                        {
                            isMemberOfLgbt = true;
                            if (therapist.IsMemberOfLGBT != null && therapist.IsMemberOfLGBT == true)
                            {
                                score += 2.0;
                            }
                        }
                        else if (value == "Sa nu fie religios")
                        {
                            isNotReligious = true;
                            if (therapist.IsNotReligious != null && therapist.IsNotReligious == true)
                            {
                                score += 2.0;
                            }
                        }
                        else if (value == "Sa fie de etnie rroma")
                        {
                            isRroma = true;
                            if (therapist.IsRRoma != null && therapist.IsRRoma == true)
                            {
                                score += 2.0;
                            }
                        }
                        else if (value == "Sa fie de etnie maghiara")
                        {
                            isHungarian = true;
                            if (therapist.IsHungarian != null && therapist.IsHungarian == true)
                            {
                                score += 2.0;
                            }
                        }
                        else if (value == "Sa aiba sub 35 de ani")
                        {
                            under35 = true;
                            if (therapist.Age != null && therapist.Age <= 35)
                                score += 2.0;
                            else if (therapist.Age != null && therapist.Age <= 40)
                                score += 0.5;
                        }
                        else if (value == "Sa aiba peste 55 de ani")
                        {
                            above55 = true;
                            if (therapist.Age != null && therapist.Age >= 55)
                                score += 2.0;
                            else if (therapist.Age != null && therapist.Age >= 50)
                                score += 0.5;
                        }

                    
                }
                else if (option.Key == "Ai dori ca terapeutul tau sa fie specializat in probleme legate de comunitatea LGBTQ+?")
                {
                    if (option.Value == "Da")
                    {
                        lgbt = true;
                    }

                }
                else if (option.Key == "Care este statusul relatiei tale?")
                {
                    if (option.Value != "Singur/a" && option.Value != "Vaduv/a")
                        relationship = true;
                    if (option.Value == "Vaduv/a")
                        widow = true;
                }
                else if (option.Key == "Cu ce religie te identifici?")
                {
                    if (option.Value == "Crestinism")
                        christianity = true;
                    else if (option.Value == "Islam")
                        islam = true;
                    else if (option.Value == "Iudaism")
                        judaism = true;

                }
                else if (option.Key == "Ai vrea ca terapia sa fie bazata pe religie?" && option.Value == "Nu")
                {
                    nonReligion = true;
                }
                    //to be added city  && availability
                }


                //check therapist specialties

                foreach (var specialtyId in specialties)
                {

                    if (lgbt && specialtyId == 8)
                    {
                        score += 1.5;
                    }
                    else if (relationship && specialtyId == 5)
                    {
                        score += 1.2;
                    }
                    else if (widow && specialtyId == 9)
                    {
                        score += 1.2;
                    }
                    else if ((christianity && specialtyId == 21) && !nonReligion)
                    {
                        score += 1.5;
                    }
                    else if ((islam && specialtyId == 22) && !nonReligion)
                    {
                        score += 1.5;
                    }
                    else if ((judaism && specialtyId == 23) && !nonReligion)
                    {
                        score += 1.5;
                    }



                }
                return score;

            }
        public async Task<List<(Therapist, double)>> ShowTherapistsScores(Dictionary<string, string> testF)
        {
            List<Therapist> therapists = await _unitOfWork.Therapists.GetAll();

            // Calculate compatibility scores for all therapists
            List<(Therapist therapist, double compatibilityScore)> compatibilityScores = new List<(Therapist, double)>();
            foreach (var therapist in therapists)
            {
                var specialties = await _unitOfWork.Therapists.GetSpecialtiesByTherapistId(therapist.TherapistId);
                var first9Elements = testF.Take(9).ToDictionary(x => x.Key, x => x.Value);
                double score = CalculateCompatibilityScore(first9Elements, therapist, specialties);

               /* //screening for depression
                var testDepression = testF.Skip(9).ToDictionary(x => x.Key, x => x.Value);
                depressionScore = CalculateDepressionScore(testDepression);*/
                compatibilityScores.Add((therapist, score));
            }
            return compatibilityScores;

        }
        
        /*    public double CalculateCompatibilityScore(Dictionary<string, string> options, Therapist therapist, List<int> specialties)
        {
            double score = 0.0;

            //preferintele pacientului
            string space = "";
            bool lgbt = false;
            bool relationship = false;
            bool religion = false;
            bool christianity = false;
            bool islam = false;
            bool judaism = false;
            bool isMemberOfLgbt = false;
            bool isReligious = false;
            bool isRroma = false;
            bool isHungarian = false;
            bool isFemale = false;
            bool isMale = false;
            bool isNotReligious = false;
            bool under35 = false;
            bool above55 = false;
            bool widow = false;
            bool nonReligion = false;
            string therapistGender = "";

            // Patient preferences
            foreach (KeyValuePair<string, string> option in options)
            {
                string key = option.Key;
                string value = option.Value;

                if (option.Key == "Ai cerinte specifice in legatura cu terapeutul?")
                {
                    if (value == "Sa fie femeie")
                    {
                        isFemale = true;
                        if (therapist.Gender != null && therapist.Gender.ToLower() == "female")
                            score += 1.7;
                        else if (therapist.Gender == null)
                            score += 0.5;
                    }
                    else if (value == "Sa fie barbat")
                    {
                        isMale = true;
                        if (therapist.Gender != null && therapist.Gender.ToLower() == "male")
                            score += 1.7;
                        else if (therapist.Gender == null)
                            score += 0.5;
                    }
                    else if (value == "Sa apartina comunitatii LGBTQ+")
                    {
                        isMemberOfLgbt = true;
                        if (therapist.IsMemberOfLGBT != null && therapist.IsMemberOfLGBT == true)
                            score += 1.7;
                    }
                    else if (value == "Sa nu fie religios")
                    {
                        isNotReligious = true;
                        if (therapist.IsNotReligious != null && therapist.IsNotReligious == true)
                            score += 1.7;
                    }
                    else if (value == "Sa fie de etnie rroma")
                    {
                        isRroma = true;
                        if (therapist.IsRRoma != null && therapist.IsRRoma == true)
                            score += 1.7;
                    }
                    else if (value == "Sa fie de etnie maghiara")
                    {
                        isHungarian = true;
                        if (therapist.IsHungarian != null && therapist.IsHungarian == true)
                            score += 1.7;
                    }
                    else if (value == "Sa aiba sub 35 de ani")
                    {
                        under35 = true;
                        if (therapist.Age != null && therapist.Age <= 35)
                            score += 1.7;
                        else if (therapist.Age != null && therapist.Age <= 40)
                            score += 0.5;
                    }
                    else if (value == "Sa aiba peste 55 de ani")
                    {
                        above55 = true;
                        if (therapist.Age != null && therapist.Age >= 55)
                            score += 1.7;
                        else if (therapist.Age != null && therapist.Age >= 50)
                            score += 0.5;
                    }
                }
                else if (option.Key == "Ai dori ca terapeutul tau sa fie specializat in probleme legate de comunitatea LGBTQ+?")
                {
                    if (option.Value == "Da")
                        lgbt = true;
                        //score += 1.0;
                }
                else if (option.Key == "Care este statusul relatiei tale?")
                {
                    if (option.Value != "Singur/a" && option.Value != "Vaduv/a")
                       // score += 1.0;
                       relationship = true;
                    if (option.Value == "Vaduv/a")
                        widow = true;
                }
                else if (option.Key == "Cu ce religie te identifici?")
                {
                    if (option.Value == "Crestinism")
                        score += 1.0;
                    else if (option.Value == "Islam")
                        score += 1.0;
                    else if (option.Value == "Iudaism")
                        score += 1.0;
                }
                else if (option.Key == "Ai vrea ca terapia sa fie bazata pe religie?" && option.Value == "Nu")
                {
                    nonReligion = true;
                }
                // To be added: city and availability
            }

            // Check therapist specialties
            foreach (var specialtyId in specialties)
            {
                if (isMemberOfLgbt && specialtyId == 8)
                    score += 1.5;
                else if (under35 && specialtyId == 5)
                    score += 1.2;
                else if (widow && specialtyId == 9)
                    score += 1.2;
                else if ((specialtyId == 21 || specialtyId == 22 || specialtyId == 23) && !nonReligion)
                    score += 1.5;
            }

            return score;
        }*/
        public int CalculateDepressionScore(Dictionary<string, string> options)
        {
            int score = 0;
            foreach (KeyValuePair<string, string> option in options)
            {
                string key = option.Key;
                string value = option.Value;
                if (value == "Deloc")
                    score += 0;
                else if (value == "In mai multe zile")
                    score += 1;
                else if (value == "In mai mult de jumatate din timp")
                    score += 2;
                else if (value == "Aproape in fiecare zi")
                    score += 3;
            }
            return score;

        }
        public async Task<List<Therapist>> GetRecommendedTherapists(Dictionary<string, string> testF, int patientId)
        {
            int depressionScore = 0;
            int anxietyScore = 0;
            //retrieving all therapists
            List<Therapist> therapists = await _unitOfWork.Therapists.GetAll();

            // Calculate compatibility scores for all therapists
            List < (Therapist therapist, double compatibilityScore) > compatibilityScores = new List<(Therapist, double)>();
            foreach (var therapist in therapists)
            {
                var specialties = await _unitOfWork.Therapists.GetSpecialtiesByTherapistId(therapist.TherapistId);
                var first9Elements = testF.Take(9).ToDictionary(x => x.Key, x => x.Value);
                double score = CalculateCompatibilityScore(first9Elements, therapist, specialties);

                //screening for depression
                var testDepression = testF.Skip(9).Take(9).ToDictionary(x => x.Key, x => x.Value);

                //screening for anxiety
                var testAnxiety = testF.Skip(18).Take(7).ToDictionary(x => x.Key, x => x.Value); 

                depressionScore = CalculateDepressionScore(testDepression);
                anxietyScore = CalculateDepressionScore(testAnxiety);
                compatibilityScores.Add((therapist, score));
            }

            // Sorting therapists based on compatibility scores in descending order
            compatibilityScores.Sort((x, y) => y.compatibilityScore.CompareTo(x.compatibilityScore));

            // Selecting the two most compatible therapists
            List<Therapist> recommendedTherapists = compatibilityScores
                .Take(2)
                .Select(x => x.therapist)
                .ToList();

            var test = new Test
            {
                PatientId = patientId,
                Questions = testF,
                DepressionScore = depressionScore,
                AnxietyScore = anxietyScore,
            };

            //add test tpo db
            await _unitOfWork.Tests.AddTest(test);
            /*  foreach(var recommendedTherapist in recommendedTherapists)
              {
                  var therapistModel = new TherapistMode
              }*/
            // Return the recommended therapists
            return recommendedTherapists;
        }

       
    }
}
