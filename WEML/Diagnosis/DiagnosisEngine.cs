using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEML.Models;
using WEML.Repos;

namespace WEML.Diagnosis
{
    public class DiagnosisEngine
    {
        private readonly SymptomsRepo _symptomsRepo;

        public DiagnosisEngine(SymptomsRepo symptomsRepo)
        {
            _symptomsRepo = symptomsRepo;
        }

        public async Task<string> GetDiagnosisAsync()
        {
            
            List<Symptom> symptoms = await _symptomsRepo.GetAllSymptomsAsync();

            if (symptoms.Any(s => s.SymptomName.Equals("Memory Loss", StringComparison.OrdinalIgnoreCase) && s.Severity.Equals("Severe", StringComparison.OrdinalIgnoreCase)) &&
                symptoms.Any(s => s.SymptomName.Equals("Confusion", StringComparison.OrdinalIgnoreCase)))
            {
                return "Possible Alzheimer's Disease or Dementia";
            }

            else if (symptoms.Any(s => s.SymptomName.Equals("Joint Pain", StringComparison.OrdinalIgnoreCase) && s.Severity.Equals("Severe", StringComparison.OrdinalIgnoreCase)) &&
                     symptoms.Any(s => s.SymptomName.Equals("Stiffness", StringComparison.OrdinalIgnoreCase)))
            {
                return "Osteoarthritis";
            }

            else if (symptoms.Any(s => s.SymptomName.Equals("Headache", StringComparison.OrdinalIgnoreCase) && s.Severity.Equals("Severe", StringComparison.OrdinalIgnoreCase)) &&
                     symptoms.Any(s => s.SymptomName.Equals("Dizziness", StringComparison.OrdinalIgnoreCase)) &&
                     symptoms.Any(s => s.SymptomName.Equals("Blurred Vision", StringComparison.OrdinalIgnoreCase)))
            {
                return "Possible Hypertension";
            }

            else if (symptoms.Any(s => s.SymptomName.Equals("Chest Pain", StringComparison.OrdinalIgnoreCase) && s.Severity.Equals("Severe", StringComparison.OrdinalIgnoreCase)) &&
                     symptoms.Any(s => s.SymptomName.Equals("Shortness of Breath", StringComparison.OrdinalIgnoreCase)) &&
                     symptoms.Any(s => s.SymptomName.Equals("Fatigue", StringComparison.OrdinalIgnoreCase)))
            {
                return "Possible Heart Disease";
            }

            else if (symptoms.Any(s => s.SymptomName.Equals("Excessive Thirst", StringComparison.OrdinalIgnoreCase)) &&
                     symptoms.Any(s => s.SymptomName.Equals("Frequent Urination", StringComparison.OrdinalIgnoreCase)) &&
                     symptoms.Any(s => s.SymptomName.Equals("Fatigue", StringComparison.OrdinalIgnoreCase)))
            {
                return "Possible Diabetes";
            }

            else if (symptoms.Any(s => s.SymptomName.Equals("Numbness", StringComparison.OrdinalIgnoreCase)) &&
                     symptoms.Any(s => s.SymptomName.Equals("Speech Difficulty", StringComparison.OrdinalIgnoreCase)) &&
                     symptoms.Any(s => s.SymptomName.Equals("Weakness", StringComparison.OrdinalIgnoreCase)))
            {
                return "Possible Stroke";
            }

            else if (symptoms.Any(s => s.SymptomName.Equals("Blurred Vision", StringComparison.OrdinalIgnoreCase)) &&
                     symptoms.Any(s => s.SymptomName.Equals("Difficulty Seeing at Night", StringComparison.OrdinalIgnoreCase)))
            {
                return "Cataracts";
            }
            else if (symptoms.Any(s => s.SymptomName.Equals("Frequent Urination", StringComparison.OrdinalIgnoreCase)) &&
                     symptoms.Any(s => s.SymptomName.Equals("Painful Urination", StringComparison.OrdinalIgnoreCase)) &&
                     symptoms.Any(s => s.SymptomName.Equals("Confusion", StringComparison.OrdinalIgnoreCase)))
            {
                return "Possible Urinary Tract Infection (UTI)";
            }

            else if (symptoms.Any(s => s.SymptomName.Equals("Fever", StringComparison.OrdinalIgnoreCase)) &&
                     symptoms.Any(s => s.SymptomName.Equals("Shortness of Breath", StringComparison.OrdinalIgnoreCase)) &&
                     symptoms.Any(s => s.SymptomName.Equals("Chest Pain", StringComparison.OrdinalIgnoreCase)))
            {
                return "Possible Pneumonia";
            }

            else if (symptoms.Any(s => s.SymptomName.Equals("Fever", StringComparison.OrdinalIgnoreCase)) &&
                     symptoms.Any(s => s.SymptomName.Equals("Cough", StringComparison.OrdinalIgnoreCase)) &&
                     symptoms.Any(s => s.SymptomName.Equals("Fatigue", StringComparison.OrdinalIgnoreCase)))
            {
                return "Flu";
            }

            else if (symptoms.Any(s => s.SymptomName.Equals("Joint Pain", StringComparison.OrdinalIgnoreCase) && s.Severity.Equals("Moderate", StringComparison.OrdinalIgnoreCase)) &&
                     symptoms.Any(s => s.SymptomName.Equals("Fatigue", StringComparison.OrdinalIgnoreCase) && s.Severity.Equals("Severe", StringComparison.OrdinalIgnoreCase)) &&
                     symptoms.Any(s => s.SymptomName.Equals("Memory Loss", StringComparison.OrdinalIgnoreCase) && s.Severity.Equals("Mild", StringComparison.OrdinalIgnoreCase)))
            {
                return "Possible Osteoarthritis with Chronic Fatigue or Early Stage Dementia";
            }
            else if (symptoms.Any(s => s.SymptomName.Equals("Chest Pain", StringComparison.OrdinalIgnoreCase) && s.Severity.Equals("Severe", StringComparison.OrdinalIgnoreCase)) &&
                     symptoms.Any(s => s.SymptomName.Equals("Nausea", StringComparison.OrdinalIgnoreCase) && s.Severity.Equals("Moderate", StringComparison.OrdinalIgnoreCase)) &&
                     symptoms.Any(s => s.SymptomName.Equals("Dizziness", StringComparison.OrdinalIgnoreCase)))
            {
                return "Possible Gastroesophageal Reflux Disease (GERD) or Heart Attack";
            }

            else
            {
                return "Diagnosis not available or too vague";
            }
        }
    }
}
