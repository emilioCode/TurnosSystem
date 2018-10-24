import { Component,Inject, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import Swal from 'sweetalert2';
import { FormGroup, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
    
  tipoServicio:string;
  services:any=[];
  tickets:any=[];
  ticketsFinishes:any=[];
  ticketsFinishesAccount=[];
  TicketsforUser:any=[];

  url:string;
  formGroup:FormGroup;
  headers = new HttpHeaders().set("content-type", "application/json");

  //userAccount={};
  userAccount:any=null;

  constructor(private http:HttpClient, @Inject('BASE_URL') baseUrl: string, private formBuilder: FormBuilder) { 
    this.url= baseUrl;
    http.get(baseUrl + 'api/Servicios').subscribe(result => {
      this.services = result;
      //console.table(this.services);
    }, error => console.error(error));
  }
  
  ngOnInit() {
    this.formGroup = this.formBuilder.group({
      user:'',
      pwd:''
    });
  }

  logearse(/*user:string,pwd:string*/){
    
    let usuario = Object.assign({},this.formGroup.value);
    //console.table(usuario);
    if(usuario.user == "" || usuario.user == null){
      Swal('AVISO', 'Introduzca el usuario', 'warning');
    }else{
      
      this.http.post(this.url + 'api/Servicios/Logearse', usuario, { headers: this.headers, responseType:'text' })
      .subscribe(res=>{
        //console.log(res);
        this.userAccount = JSON.parse(res);
        //console.log('userAccount')
        //console.table(this.userAccount);

       if(this.userAccount !=null){
        this.getTickets();
        this.getTicketsFinishes();
       }else{
        Swal('Acceso denegado','El usuario o la contraseña no coinciden.','error')
       }
      },
      error => {
        Swal('Failed','Ha ocurrido un error','error')
        console.error(error);
      });
    }

    this.formGroup.reset();
  }

  cerrarSesion(){
    this.userAccount = null;
  }

  setTurno(v:any){
    const headers = new HttpHeaders().set("content-type", "application/json");
    
    this.http.post(this.url + 'api/Servicios',v,{headers:headers,responseType:'text'})
      .subscribe(res=> {
        Swal(res.split(' ')[0],'tiempo estimado: '+ res.split(' ')[1] + ' minutos','info');
        this.getTickets()
      }, error => console.error(error));

      this.tipoServicio='';

  }

  getTickets(){
    this.tickets=[];
    this.http.post(this.url+'api/Servicios/getTickets',null).subscribe(res=>{
      //console.log('getTickets');
      //console.table(res);
      this.tickets = res;

    },error=>{
      console.error(error);
    });
  }

  deshabilitar(id:number){
  
    var any:any = JSON.stringify(id);
    this.http.post(this.url + 'api/Servicios/deshabilitar',any,{ headers:this.headers, responseType:'text' })
    .subscribe(res=> {
      this.getTickets();//cargar tickets
      
    },error=>console.error(error));

  }

  atender(ticket:any, user:any,tipoServicio:any){
    var data ={
      ticket:ticket,
      user:user
    }


    this.http.post(this.url+ 'api/Servicios/atender',data,{headers:this.headers,responseType:'text'})
      .subscribe(res=>{
        var data = JSON.parse(res)
        Swal(tipoServicio, 'TURNO:' + data.turno, 'success');
        this.getTickets();//cargar tickets
        this.getTicketsFinishes();//cargar ticket por despachar
      },error=>console.error(error))
  }

  getTicketsFinishes(){
    this.ticketsFinishesAccount=[];
    this.http.post(this.url + 'api/Servicios/getTicketsFinishes',null,{headers:this.headers,responseType:'json'})
    .subscribe(res=>{
      this.ticketsFinishes=res;
      this.ticketsFinishes.forEach(element => {
        if(element.usuario == this.userAccount.id) this.ticketsFinishesAccount.push(element);
      });

      console.table(this.ticketsFinishes);
      console.table(this.ticketsFinishesAccount);
    },error=>console.error(error))
  }

  despachar(ticket:any){
    //console.log(ticket)
    this.http.post(this.url + 'api/Servicios/despachar', ticket,{headers:this.headers, responseType:'text'})
    .subscribe(res=>{
      this.getTicketsFinishes();
    },error=>console.error(error))
  }

  getTicketsforUser(){
    this.http.post(this.url + 'api/Servicios/getTicketsforUser',null,{headers:this.headers, responseType:'json'})
    .subscribe(res=> {
      this.TicketsforUser = res;
      //console.table(res);
    },error=>console.error(error))

  }



 

}//End