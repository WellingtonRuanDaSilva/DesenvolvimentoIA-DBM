import requests
from dataclasses import dataclass
from typing import Optional
from urllib3.exceptions import InsecureRequestWarning

# Desabilitar avisos de certificado SSL não verificado
requests.packages.urllib3.disable_warnings(InsecureRequestWarning)

@dataclass
class Person:
    id: int
    nome: str
    idade: int
    cidade: str
    profissao: str

class AgeCategory:
    @staticmethod
    def categorize(age: int) -> str:
        if age < 30:
            return "Jovem"
        elif age <= 40:
            return "Adulto"
        else:
            return "Sênior"

class PersonClient:
    def __init__(self, base_url: str):
        self.base_url = base_url.rstrip('/')

    def get_person(self, person_id: int) -> Optional[Person]:
        try:
            response = requests.get(
                f"{self.base_url}/api/Person/{person_id}",
                verify=False  # Desabilitar verificação SSL para desenvolvimento local
            )
            
            if response.status_code == 200:
                data = response.json()
                return Person(**data)
            elif response.status_code == 404:
                print(f"Pessoa com ID {person_id} não encontrada.")
                return None
            else:
                print(f"Erro na requisição: Status {response.status_code}")
                return None
                
        except requests.RequestException as e:
            print(f"Erro ao fazer a requisição: {str(e)}")
            return None

def display_person(person: Person):
    category = AgeCategory.categorize(person.idade)
    print("\n=== Informações da Pessoa ===")
    print(f"Categoria: {category}")
    print(f"ID: {person.id}")
    print(f"Nome: {person.nome}")
    print(f"Idade: {person.idade} anos")
    print(f"Cidade: {person.cidade}")
    print(f"Profissão: {person.profissao}")
    print("==========================\n")

def main():
    # Use a porta correta do seu servidor local
    api_url = "https://localhost:7223"  # Ajuste esta porta para a que seu servidor está usando
    client = PersonClient(api_url)
    
    while True:
        try:
            id_input = input("Digite o ID da pessoa (ou 'q' para sair): ")
            
            if id_input.lower() == 'q':
                break
                
            person_id = int(id_input)
            person = client.get_person(person_id)
            
            if person:
                display_person(person)
                
        except ValueError:
            print("Por favor, digite um número válido.")
        except Exception as e:
            print(f"Erro inesperado: {str(e)}")
            break

if __name__ == "__main__":
    main()