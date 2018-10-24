import { Component,Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import Swal  from 'sweetalert2';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {

  services:any=[];
  tipoServicio:any='';
  url:string;

  constructor(public http:HttpClient, @Inject('BASE_URL') baseUrl: string){ 
    this.url=baseUrl;
    http.get(baseUrl + 'api/Servicios').subscribe(result => {
      this.services = result;
      //console.table(this.services);
    }, error => console.error(error));
    
  }

  setTurno(v:any){
    //console.log('valor: '+ v);
    //return this.http.post(this.url + 'api/Pais', nombre, { headers: headers, responseType:'text' });
    const headers = new HttpHeaders().set("content-type", "application/json");
    
    var str = JSON.stringify(v);
    this.http.post(this.url + 'api/Servicios',str,{headers:headers,responseType:'text'})
      .subscribe(res=> {
        Swal(res.split(' ')[0],'tiempo estimado: '+ res.split(' ')[1] + ' minutos','info');
      }, error => console.error(error));

      this.tipoServicio='';

  }


}
