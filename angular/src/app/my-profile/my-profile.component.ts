import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { RestService } from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';

@Component({
    selector: 'app-my-profile',
    templateUrl: './my-profile.component.html',
    standalone: false
})
export class MyProfileComponent implements OnInit {
    activeTab = 'personal';
    form: FormGroup;
    passwordForm: FormGroup;
    isLoading = false;

    constructor(
        private fb: FormBuilder,
        private rest: RestService,
        private toaster: ToasterService
    ) { }

    ngOnInit(): void {
        this.buildForms();
        this.getProfile();
    }

    buildForms() {
        this.form = this.fb.group({
            userName: [{ value: '', disabled: true }, Validators.required],
            email: [{ value: '', disabled: true }, [Validators.required, Validators.email]],
            name: ['', Validators.required],
            surname: ['', Validators.required],
            phoneNumber: ['', Validators.required],
        });

        this.passwordForm = this.fb.group({
            currentPassword: ['', Validators.required],
            newPassword: ['', [Validators.required, Validators.minLength(6)]],
            newPasswordConfirm: ['', Validators.required]
        }, { validators: this.passwordMatchValidator });
    }

    passwordMatchValidator(g: FormGroup) {
        return g.get('newPassword').value === g.get('newPasswordConfirm').value
            ? null : { mismatch: true };
    }

    getProfile() {
        this.isLoading = true;
        this.rest.request({
            method: 'GET',
            url: '/api/account/my-profile'
        }).subscribe({
            next: (res: any) => {
                this.form.patchValue(res);
                this.isLoading = false;
            },
            error: () => this.isLoading = false
        });
    }

    saveProfile() {
        if (this.form.invalid) return;

        this.isLoading = true;
        this.rest.request({
            method: 'PUT',
            url: '/api/account/my-profile',
            body: this.form.getRawValue()
        }).subscribe({
            next: (res) => {
                this.toaster.success('Profile updated successfully.', 'Success');
                this.isLoading = false;
                // Optionally update global config or state if needed
            },
            error: (err) => {
                this.isLoading = false;
                this.toaster.error(err.error?.error?.message || 'An error occurred.', 'Error');
            }
        });
    }

    changePassword() {
        if (this.passwordForm.invalid) return;

        this.isLoading = true;
        this.rest.request({
            method: 'POST',
            url: '/api/account/my-profile/change-password',
            body: this.passwordForm.value
        }).subscribe({
            next: () => {
                this.toaster.success('Password changed successfully.', 'Success');
                this.passwordForm.reset();
                this.isLoading = false;
            },
            error: (err) => {
                this.isLoading = false;
                this.toaster.error(err.error?.error?.message || 'Could not change password.', 'Error');
            }
        });
    }
}
