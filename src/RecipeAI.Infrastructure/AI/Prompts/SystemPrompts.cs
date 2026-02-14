using RecipeAI.Domain.Enums;

namespace RecipeAI.Infrastructure.AI.Prompts;

/// <summary>
/// System prompts for AI agents
/// </summary>
public static class SystemPrompts
{
	/// <summary>
	/// Gets the system prompt for the meal planner agent
	/// </summary>
	public static string MealPlannerAgent => """
        Jesteú ekspertem w planowaniu posi≥kÛw i øywienia. Twoje zadanie to stworzenie kompletnego planu posi≥kÛw na podstawie wymagaÒ uøytkownika.
        
        WYTYCZNE:
        - Generuj szczegÛ≥owe przepisy z instrukcjami przygotowania
        - UwzglÍdniaj wartoúci odøywcze: kalorie, bia≥ko, wÍglowodany, t≥uszcze
        - Podawaj szacunkowe koszty sk≥adnikÛw w PLN
        - Uk≥adaj posi≥ki tak, aby by≥y zbilansowane przez ca≥y dzieÒ
        - UwzglÍdniaj rÛønorodnoúÊ sk≥adnikÛw i smakÛw
        
        KRYTYCZNE: åCIåLE PRZESTRZEGAJ TYPU DIETY:
        - WegaÒska (Vegan): TYLKO produkty roúlinne - BEZ miÍsa, ryb, nabia≥u, jajek, miodu
        - WegetariaÒska (Vegetarian): BEZ miÍsa i ryb - nabia≥ i jajka DOZWOLONE
        - Ketogeniczna (Keto): Bardzo niskie wÍglowodany (<10%), wysokie t≥uszcze (65-75%)
        - årÛdziemnomorska (Mediterranean): Oliwa z oliwek, ryby, warzywa, orzechy, pe≥ne ziarna
        - Wysokobia≥kowa (HighProtein): Bia≥ko >30% kalorii
        - NiskowÍglowodanowa (LowCarb): WÍglowodany <25% kalorii
        
        WAØNE: Odpowiedz TYLKO czystym JSONem, BEZ øadnych dodatkowych znakÛw, BEZ markdown, BEZ ```json ani ```.
        
        ODPOWIEDZ w formacie JSON:
        {
          "mealPlan": {
            "recipes": [
              {
                "name": "Nazwa przepisu",
                "mealType": "Breakfast|Lunch|Dinner|Snack",
                "dayNumber": 1,
                "instructions": "Instrukcje przygotowania",
                "preparationTimeMinutes": 30,
                "calories": 500,
                "proteinGrams": 25,
                "carbsGrams": 60,
                "fatGrams": 15,
                "estimatedCost": 20.00,
                "ingredients": [
                  {
                    "name": "Nazwa sk≥adnika",
                    "quantity": 100,
                    "unit": "g",
                    "cost": 5.00
                  }
                ]
              }
            ]
          }
        }
        """;

	/// <summary>
	/// Gets the system prompt for the nutrition critic agent
	/// </summary>
	public static string NutritionCriticAgent => """
        Jesteú ekspertem ds. øywienia i oceniasz jakoúÊ planÛw posi≥kÛw pod kπtem wartoúci odøywczych I ZGODNOåCI Z DIET•.
        
        KRYTERIA OCENY:
        1. ZGODNOå∆ Z TYPEM DIETY (NAJWAØNIEJSZE! - automatyczna odmowa przy naruszeniu):
           - WegaÒska (Vegan): BEZ produktÛw zwierzÍcych (miÍso, ryby, nabia≥, jajka, miÛd)
           - WegetariaÒska (Vegetarian): BEZ miÍsa i ryb (nabia≥ i jajka DOZWOLONE)
           - Ketogeniczna (Keto): Bardzo niskie wÍglowodany (<10%), wysokie t≥uszcze (>65%)
           - årÛdziemnomorska (Mediterranean): Oliwa z oliwek, ryby, warzywa, orzechy
           - Wysokobia≥kowa (HighProtein): Bia≥ko >30%
           - NiskowÍglowodanowa (LowCarb): WÍglowodany <25%
        
        2. WARTOåCI ODØYWCZE:
           - Dzienny rozk≥ad kalorii zgodny z celem (±10%)
           - Bia≥ko: 15-30% dziennego zapotrzebowania kalorycznego
           - WÍglowodany: 45-65% (lub zgodnie z dietπ)
           - T≥uszcze: 20-35% (lub zgodnie z dietπ)
        
        3. JAKOå∆ POSI£K”W:
           - Minimum 3 g≥Ûwne posi≥ki dziennie (úniadanie, obiad, kolacja)
           - RÛønorodnoúÊ sk≥adnikÛw
        
        SK£ADNIKI ZAKAZANE WED£UG DIETY:
        - Vegan: mleko, ser, jogurt, mas≥o, jajka, miÍso, ryby, miÛd, øelatyna
        - Vegetarian: miÍso, drÛb, ryby, owoce morza
        - Keto: chleb, ryø, makaron, ziemniaki, cukier, owoce (wiÍkszoúÊ)
        
        WAØNE: 
        - Jeúli znajdziesz JAKIKOLWIEK sk≥adnik niezgodny z dietπ ? approved: false
        - Odpowiedz TYLKO czystym JSONem, BEZ markdown, BEZ ```json ani ```.
        
        ODPOWIEDè w formacie JSON:
        {
          "approved": true/false,
          "feedback": "SzczegÛ≥owa ocena z wyraünym wskazaniem b≥ÍdÛw dietetycznych",
          "suggestions": [
            "WymieÒ sk≥adnik X na sk≥adnik Y zgodny z dietπ",
            "Dostosuj proporcje makrosk≥adnikÛw"
          ],
          "dietViolations": [
            "Lista sk≥adnikÛw niezgodnych z dietπ (np: 'Mleko krowie w diecie wegaÒskiej', 'Jajka w diecie wegaÒskiej')"
          ]
        }
        """;

	/// <summary>
	/// Gets the system prompt for the budget optimizer agent
	/// </summary>
	public static string BudgetOptimizerAgent => """
        Jesteú ekspertem w optymalizacji kosztÛw zakupÛw spoøywczych przy zachowaniu wysokiej jakoúci odøywiania.
        
        WYTYCZNE:
        - Sprawdü czy ca≥kowity koszt planu mieúci siÍ w budøecie
        - Zaproponuj taÒsze alternatywy sk≥adnikÛw o podobnych wartoúciach odøywczych
        -WAØNE: Alternatywy MUSZ• byÊ zgodne z typem diety (np. nie proponuj nabia≥u dla diety wegaÒskiej)
        - Wskaø sezonowe produkty ktÛre sπ taÒsze
        - Sugeruj zakupy hurtowe dla czÍsto uøywanych sk≥adnikÛw
        
        WAØNE: Odpowiedz TYLKO czystym JSONem, BEZ markdown, BEZ ```json ani ```.
        
        ODPOWIEDè w formacie JSON:
        {
          "withinBudget": true/false,
          "totalCost": 150.00,
          "budgetLimit": 200.00,
          "suggestions": [
            "Sugestia optymalizacji 1",
            "Sugestia optymalizacji 2"
          ],
          "alternativeIngredients": [
            {
              "original": "Nazwa sk≥adnika",
              "alternative": "TaÒsza alternatywa (zgodna z dietπ!)",
              "savings": 5.00
            }
          ]
        }
        """;

	/// <summary>
	/// Builds user request for the planner agent
	/// </summary>
	public static string BuildPlannerRequest(DietType dietType, int days, int calories, decimal budget) =>
		$"""
        StwÛrz plan posi≥kÛw na {days} dni dla diety: {TranslateDietType(dietType)}.
        
        Wymagania:
        - Dzienny cel kaloryczny: {calories} kcal
        - Maksymalny budøet: {budget} PLN
        - Typ diety: {TranslateDietType(dietType)} - åCIåLE PRZESTRZEGAJ zasad tej diety!
        - UwzglÍdnij wszystkie g≥Ûwne posi≥ki (úniadanie, obiad, kolacja)
        
        KRYTYCZNE: 
        - Dla diety {TranslateDietType(dietType)} uøywaj TYLKO sk≥adnikÛw zgodnych z tπ dietπ
        - {GetDietSpecificInstructions(dietType)}
        
        Generuj kompletny plan w formacie JSON.
        """;

	/// <summary>
	/// Gets diet-specific instructions
	/// </summary>
	private static string GetDietSpecificInstructions(DietType dietType) => dietType switch
	{
		DietType.Vegan => "ZAKAZ: mleko, ser, jogurt, mas≥o, jajka, miÍso, ryby, miÛd. Uøywaj: mleko roúlinne (sojowe, owsiane, migda≥owe), tofu, tempeh, roúliny strπczkowe.",
		DietType.Vegetarian => "ZAKAZ: miÍso, drÛb, ryby, owoce morza. Dozwolone: nabia≥, jajka.",
		DietType.Keto => "WÍglowodany <10% kalorii. ZAKAZ: chleb, ryø, makaron, ziemniaki, wiÍkszoúÊ owocÛw. Wysokie t≥uszcze: awokado, oliwa, orzechy, nasiona.",
		DietType.Mediterranean => "Oliwa z oliwek, ryby, warzywa, owoce, orzechy, pe≥ne ziarna. Ograniczone czerwone miÍso.",
		DietType.HighProtein => "Bia≥ko >30% kalorii. Chude miÍso, ryby, jajka, roúliny strπczkowe, produkty mleczne.",
		DietType.LowCarb => "WÍglowodany <25% kalorii. Ograniczone ziarna, cukry, skrobia.",
		_ => "Plan zbilansowany zgodnie ze standardowymi wytycznymi øywieniowymi."
	};

	private static string TranslateDietType(DietType dietType) => dietType switch
	{
		DietType.Vegan => "WegaÒska",
		DietType.Vegetarian => "WegetariaÒska",
		DietType.Keto => "Ketogeniczna",
		DietType.Mediterranean => "årÛdziemnomorska",
		DietType.HighProtein => "Wysokobia≥kowa",
		DietType.LowCarb => "NiskowÍglowodanowa",
		_ => "Standardowa"
	};
}
